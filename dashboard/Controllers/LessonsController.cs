using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HorofDashboard.Models;
using HorofDashboard.ViewModels;

namespace HorofDashboard.Controllers
{
    public class LessonsController : Controller
    {
        private readonly HorofContentContext _context;

        public LessonsController(HorofContentContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index(int? unitId, int? levelId)
        {
            var query = _context.Lessons
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Level)
                .Include(l => l.Slides)
                .AsQueryable();

            if (unitId.HasValue)
            {
                query = query.Where(l => l.UnitId == unitId.Value);
                ViewBag.UnitFilter = await _context.Units
                    .Include(u => u.Level)
                    .FirstOrDefaultAsync(u => u.Id == unitId.Value);
            }
            else if (levelId.HasValue)
            {
                query = query.Where(l => l.Unit.LevelId == levelId.Value);
                ViewBag.LevelFilter = await _context.Levels.FindAsync(levelId.Value);
            }

            var lessons = await query
                .OrderBy(l => l.Unit.Level.Position)
                .ThenBy(l => l.Unit.Position)
                .ThenBy(l => l.Position)
                .ToListAsync();

            ViewBag.Units  = await _context.Units
                .Include(u => u.Level)
                .OrderBy(u => u.Level.Position)
                .ThenBy(u => u.Position)
                .ToListAsync();

            ViewBag.Levels = await _context.Levels.OrderBy(l => l.Position).ToListAsync();
            return View(lessons);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Level)
                .Include(l => l.Slides)
                    .ThenInclude(s => s.MasterSlideType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lesson == null) return NotFound();

            return View(lesson);
        }

        // GET: Lessons/Create
        public async Task<IActionResult> Create(int? unitId)
        {
            var model = new LessonCreateViewModel
            {
                Status = true,
                UnitId = unitId ?? 0
            };

            if (unitId.HasValue)
                model.Position = await GetNextPositionInUnit(unitId.Value);

            await PopulateUnitsDropdown(unitId);
            return View(model);
        }

        // POST: Lessons/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lesson = new Lesson
                    {
                        Name        = model.Name,
                        Description = model.Description,
                        Details     = model.Details,
                        Status      = model.Status,
                        MinutesWork = model.MinutesWork,
                        Position    = model.Position ?? await GetNextPositionInUnit(model.UnitId),
                        UnitId      = model.UnitId,
                        Url         = model.Url ?? string.Empty,
                        CreatedAt   = DateTime.Now
                    };

                    _context.Add(lesson);
                    await _context.SaveChangesAsync();
                    await SyncLessonsCount(model.UnitId);

                    TempData["SuccessMessage"] = "Lesson created successfully!";
                    return RedirectToAction(nameof(Index), new { unitId = model.UnitId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the lesson: " + ex.Message);
                }
            }

            await PopulateUnitsDropdown(model.UnitId);
            return View(model);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Level)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null) return NotFound();

            var model = new LessonEditViewModel
            {
                Id          = lesson.Id,
                Name        = lesson.Name,
                Description = lesson.Description,
                Details     = lesson.Details,
                Status      = lesson.Status,
                MinutesWork = lesson.MinutesWork,
                Position    = lesson.Position,
                UnitId      = lesson.UnitId,
                Url         = lesson.Url
            };

            await PopulateUnitsDropdown(lesson.UnitId);
            return View(model);
        }

        // POST: Lessons/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var lesson = await _context.Lessons.FindAsync(id);
                    if (lesson == null) return NotFound();

                    var oldUnitId = lesson.UnitId;

                    lesson.Name        = model.Name;
                    lesson.Description = model.Description;
                    lesson.Details     = model.Details;
                    lesson.Status      = model.Status;
                    lesson.MinutesWork = model.MinutesWork;
                    lesson.Position    = model.Position ?? lesson.Position;
                    lesson.UnitId      = model.UnitId;
                    lesson.Url         = model.Url ?? string.Empty;

                    _context.Update(lesson);
                    await _context.SaveChangesAsync();

                    if (oldUnitId != model.UnitId)
                        await SyncLessonsCount(oldUnitId);
                    await SyncLessonsCount(model.UnitId);

                    TempData["SuccessMessage"] = "Lesson updated successfully!";
                    return RedirectToAction(nameof(Index), new { unitId = model.UnitId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(model.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the lesson: " + ex.Message);
                }
            }

            await PopulateUnitsDropdown(model.UnitId);
            return View(model);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Level)
                .Include(l => l.Slides)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lesson == null) return NotFound();

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var lesson = await _context.Lessons
                    .Include(l => l.Unit)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lesson != null)
                {
                    var unitId = lesson.UnitId;
                    _context.Lessons.Remove(lesson);
                    await _context.SaveChangesAsync();
                    await SyncLessonsCount(unitId);

                    TempData["SuccessMessage"] = "Lesson deleted successfully!";
                    return RedirectToAction(nameof(Index), new { unitId = unitId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting lesson. Make sure all slides inside it are deleted first. Details: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id) =>
            _context.Lessons.Any(e => e.Id == id);

        private async Task<int> GetNextPositionInUnit(int unitId)
        {
            var max = await _context.Lessons
                .Where(l => l.UnitId == unitId)
                .MaxAsync(l => (int?)l.Position) ?? 0;
            return max + 1;
        }

        private async Task SyncLessonsCount(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit != null)
            {
                unit.LessonsCount = await _context.Lessons.CountAsync(l => l.UnitId == unitId);
                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateUnitsDropdown(int? selectedUnitId = null)
        {
            var units = await _context.Units
                .Include(u => u.Level)
                .Where(u => u.Status)
                .OrderBy(u => u.Level.Position)
                .ThenBy(u => u.Position)
                .ToListAsync();

            ViewBag.Units = units.Select(u => new SelectListItem
            {
                Value    = u.Id.ToString(),
                Text     = $"{u.Level.Name} → {u.Name}",
                Selected = u.Id == selectedUnitId
            }).ToList();
        }
    }
}
