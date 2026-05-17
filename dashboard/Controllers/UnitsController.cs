using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HorofDashboard.Models;
using HorofDashboard.ViewModels;

namespace HorofDashboard.Controllers
{
    public class UnitsController : Controller
    {
        private readonly HorofContentContext _context;

        public UnitsController(HorofContentContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index(int? levelId)
        {
            var query = _context.Units
                .Include(u => u.Level)
                .Include(u => u.Lessons)
                .AsQueryable();

            if (levelId.HasValue)
            {
                query = query.Where(u => u.LevelId == levelId.Value);
                ViewBag.LevelFilter = await _context.Levels.FindAsync(levelId.Value);
            }

            var units = await query.OrderBy(u => u.Level.Position).ThenBy(u => u.Position).ToListAsync();
            ViewBag.Levels = await _context.Levels.OrderBy(l => l.Position).ToListAsync();
            return View(units);
        }

        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units
                .Include(u => u.Level)
                .Include(u => u.Lessons)
                    .ThenInclude(l => l.Slides)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (unit == null) return NotFound();

            return View(unit);
        }

        // GET: Units/Create
        public async Task<IActionResult> Create(int? levelId)
        {
            var model = new UnitCreateViewModel
            {
                Status  = true,
                LevelId = levelId ?? 0
            };

            if (levelId.HasValue)
                model.Position = await GetNextPositionInLevel(levelId.Value);

            await PopulateLevelsDropdown(levelId);
            return View(model);
        }

        // POST: Units/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var unit = new Unit
                    {
                        Name         = model.Name,
                        Description  = model.Description,
                        Status       = model.Status,
                        Position     = model.Position ?? await GetNextPositionInLevel(model.LevelId),
                        LevelId      = model.LevelId,
                        Url          = model.Url ?? string.Empty,
                        HoursWork    = model.HoursWork,
                        LessonsCount = 0,
                        CreatedAt    = DateTime.Now
                    };

                    _context.Add(unit);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Unit created successfully!";
                    return RedirectToAction(nameof(Index), new { levelId = model.LevelId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the unit: " + ex.Message);
                }
            }

            await PopulateLevelsDropdown(model.LevelId);
            return View(model);
        }

        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units
                .Include(u => u.Level)
                .Include(u => u.Lessons)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null) return NotFound();

            var actualLessonsCount = unit.Lessons.Count;

            var model = new UnitEditViewModel
            {
                Id           = unit.Id,
                Name         = unit.Name,
                Description  = unit.Description,
                Status       = unit.Status,
                Position     = unit.Position,
                LevelId      = unit.LevelId,
                Url          = unit.Url,
                HoursWork    = unit.HoursWork,
                LessonsCount = actualLessonsCount
            };

            await PopulateLevelsDropdown(unit.LevelId);
            return View(model);
        }

        // POST: Units/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UnitEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var unit = await _context.Units.FindAsync(id);
                    if (unit == null) return NotFound();

                    unit.Name        = model.Name;
                    unit.Description = model.Description;
                    unit.Status      = model.Status;
                    unit.Position    = model.Position ?? unit.Position;
                    unit.LevelId     = model.LevelId;
                    unit.Url         = model.Url ?? string.Empty;
                    unit.HoursWork   = model.HoursWork;

                    _context.Update(unit);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Unit updated successfully!";
                    return RedirectToAction(nameof(Index), new { levelId = model.LevelId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(model.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the unit: " + ex.Message);
                }
            }

            await PopulateLevelsDropdown(model.LevelId);
            return View(model);
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _context.Units
                .Include(u => u.Level)
                .Include(u => u.Lessons)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (unit == null) return NotFound();

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var unit = await _context.Units
                    .Include(u => u.Level)
                    .Include(u => u.Lessons)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (unit != null)
                {
                    if (unit.Lessons.Any())
                    {
                        TempData["ErrorMessage"] = $"Cannot delete unit '{unit.Name}' because it still has {unit.Lessons.Count} lesson(s). Delete all lessons first.";
                        return RedirectToAction(nameof(Delete), new { id = id });
                    }

                    var levelId = unit.LevelId;
                    _context.Units.Remove(unit);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Unit deleted successfully!";
                    return RedirectToAction(nameof(Index), new { levelId = levelId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting unit: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id) =>
            _context.Units.Any(e => e.Id == id);

        private async Task<int> GetNextPositionInLevel(int levelId)
        {
            var max = await _context.Units
                .Where(u => u.LevelId == levelId)
                .MaxAsync(u => (int?)u.Position) ?? 0;
            return max + 1;
        }

        private async Task PopulateLevelsDropdown(int? selectedLevelId = null)
        {
            var levels = await _context.Levels
                .Where(l => l.Status)
                .OrderBy(l => l.Position)
                .ToListAsync();

            ViewBag.Levels = new SelectList(levels, "Id", "Name", selectedLevelId);
        }
    }
}
