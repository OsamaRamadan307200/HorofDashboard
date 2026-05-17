using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HorofDashboard.Models;
using HorofDashboard.ViewModels;

namespace HorofDashboard.Controllers
{
    public class LevelsController : Controller
    {
        private readonly HorofContentContext _context;
        private readonly IWebHostEnvironment _environment;

        public LevelsController(HorofContentContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Levels
        public async Task<IActionResult> Index()
        {
            var levels = await _context.Levels
                .Include(l => l.Units)
                .OrderBy(l => l.Position)
                .ToListAsync();

            return View(levels);
        }

        // GET: Levels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Levels
                .Include(l => l.Units)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (level == null)
            {
                return NotFound();
            }

            return View(level);
        }

        // GET: Levels/Create
        public IActionResult Create()
        {
            var model = new LevelCreateViewModel
            {
                Status = true,
                Position = GetNextPosition()
            };
            return View(model);
        }

        // POST: Levels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LevelCreateViewModel model, IFormFile? iconFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle file upload
                    string? iconPath = null;
                    if (iconFile != null && iconFile.Length > 0)
                    {
                        iconPath = await SaveIconFile(iconFile);
                    }

                    var level = new Level
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Status = model.Status,
                        Position = model.Position ?? GetNextPosition(),
                        PicIcon = iconPath ?? model.Pic_Icon,
                        Url = model.Url,
                        CreatedAt = DateTime.Now
                    };

                    _context.Add(level);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Level created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the level: " + ex.Message);
                }
            }

            model.Position ??= GetNextPosition();
            return View(model);
        }

        // GET: Levels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Levels.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }

            var model = new LevelEditViewModel
            {
                Id = level.Id,
                Name = level.Name,
                Description = level.Description,
                Status = level.Status,
                Position = level.Position,
                Pic_Icon = level.PicIcon,
                Url = level.Url,
                CurrentIconPath = level.PicIcon
            };

            return View(model);
        }

        // POST: Levels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LevelEditViewModel model, IFormFile? iconFile)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var level = await _context.Levels.FindAsync(id);
                    if (level == null)
                    {
                        return NotFound();
                    }

                    // Handle file upload
                    if (iconFile != null && iconFile.Length > 0)
                    {
                        // Delete old icon if exists
                        if (!string.IsNullOrEmpty(level.PicIcon))
                        {
                            DeleteIconFile(level.PicIcon);
                        }
                        level.PicIcon = await SaveIconFile(iconFile);
                    }
                    else if (!string.IsNullOrEmpty(model.Pic_Icon))
                    {
                        level.PicIcon = model.Pic_Icon;
                    }

                    level.Name = model.Name;
                    level.Description = model.Description;
                    level.Status = model.Status;
                    level.Position = model.Position;
                    level.Url = model.Url;

                    _context.Update(level);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Level updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LevelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the level: " + ex.Message);
                }
            }

            model.CurrentIconPath = model.Pic_Icon;
            return View(model);
        }

        // GET: Levels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Levels
                .Include(l => l.Units)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (level == null)
            {
                return NotFound();
            }

            return View(level);
        }

        // POST: Levels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var level = await _context.Levels.FindAsync(id);
                if (level != null)
                {
                    // Delete icon file if exists
                    if (!string.IsNullOrEmpty(level.PicIcon))
                    {
                        DeleteIconFile(level.PicIcon);
                    }

                    _context.Levels.Remove(level);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Level deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting level: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LevelExists(int id)
        {
            return _context.Levels.Any(e => e.Id == id);
        }

        private int GetNextPosition()
        {
            var maxPosition = _context.Levels.Max(l => (int?)l.Position) ?? 0;
            return maxPosition + 1;
        }

        private async Task<string> SaveIconFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "levels");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/images/levels/" + uniqueFileName;
        }

        private void DeleteIconFile(string iconPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(iconPath) && iconPath.StartsWith("/images/levels/"))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, iconPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception)
            {
                // Log error but don't throw - file deletion shouldn't stop the operation
            }
        }
    }
}