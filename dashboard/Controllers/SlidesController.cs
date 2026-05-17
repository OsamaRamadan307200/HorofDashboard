using HorofDashboard.Models;
using HorofDashboard.ViewModels.Slides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HorofDashboard.Controllers
{
    public class SlidesController : Controller
    {
        private readonly HorofContentContext _context;

        public SlidesController(HorofContentContext context)
        {
            _context = context;
        }

        // ─── Index ────────────────────────────────────────────────────────────────

        public async Task<IActionResult> Index(int? lessonId)
        {
            var query = _context.Slides
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Unit)
                        .ThenInclude(u => u.Level)
                .Include(s => s.MasterSlideType)
                .AsQueryable();

            if (lessonId.HasValue)
                query = query.Where(s => s.LessonId == lessonId.Value);

            ViewBag.LessonId = lessonId;
            return View(await query.OrderBy(s => s.Position).ToListAsync());
        }

        // ─── Type Selection ───────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult SelectType(int? lessonId)
        {
            var model = new SlideTypeSelectionViewModel
            {
                LessonId = lessonId ?? 0,
                SlideTypes = new List<SlideTypeOption>
                {
                    new() { TypeId = 1, Name = "Multiple Choice",  Description = "Several options, one correct answer.",            Icon = "fa-list-ul",      Action = nameof(CreateMultipleChoice) },
                    new() { TypeId = 2, Name = "Link Words",       Description = "Match items from two columns.",                   Icon = "fa-link",         Action = nameof(CreateLinkWords)      },
                    new() { TypeId = 3, Name = "True / False",     Description = "A statement that is either true or false.",       Icon = "fa-check-circle", Action = nameof(CreateTrueFalse)      },
                    new() { TypeId = 5, Name = "Write Word",       Description = "Type the correct word or phrase.",                Icon = "fa-pencil-alt",   Action = nameof(CreateWriteWord)      },
                    new() { TypeId = 6, Name = "Voice Choices",    Description = "Listen to audio, then pick the correct answer.", Icon = "fa-microphone",   Action = nameof(CreateVoiceChoices)   },
                    new() { TypeId = 7, Name = "Rich Text",        Description = "Answer using formatted text.",                    Icon = "fa-align-left",   Action = nameof(CreateRichText)       },
                    new() { TypeId = 8, Name = "Choices + Voice",  Description = "Multiple choice with voice per option.",          Icon = "fa-volume-up",    Action = nameof(CreateChoicesVoices)  },
                }
            };
            return View(model);
        }

        // ─── Multiple Choice (Type 1) ─────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateMultipleChoice(int? lessonId)
        {
            var model = new MultipleChoiceViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultipleChoice(MultipleChoiceViewModel model)
        {
            ValidateChoices(model.Choices, model.CorrectChoiceIndex, nameof(model.Choices));
            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.SlideContent = System.Text.Json.JsonSerializer.Serialize(model.Choices);
                slide.Answer       = model.CorrectChoiceIndex.ToString();
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Link Words (Type 2) ──────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateLinkWords(int? lessonId)
        {
            var model = new LinkWordsViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLinkWords(LinkWordsViewModel model)
        {
            if (model.FirstColumn.Count == 0)
                ModelState.AddModelError(nameof(model.FirstColumn), "Add at least one item to the first column.");
            if (model.SecondColumn.Count == 0)
                ModelState.AddModelError(nameof(model.SecondColumn), "Add at least one item to the second column.");
            if (model.LinkWordsAnswer.Count == 0)
                ModelState.AddModelError(nameof(model.LinkWordsAnswer), "Add at least one match.");

            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.SlideContent = System.Text.Json.JsonSerializer.Serialize(new { model.FirstColumn, model.SecondColumn });
                slide.Answer       = System.Text.Json.JsonSerializer.Serialize(model.LinkWordsAnswer);
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── True / False (Type 3) ────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateTrueFalse(int? lessonId)
        {
            var model = new TrueFalseViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrueFalse(TrueFalseViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TrueFalseAnswer))
                ModelState.AddModelError(nameof(model.TrueFalseAnswer), "Please select True or False.");

            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.Answer = model.TrueFalseAnswer!.ToLower();
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Write Word (Type 5) ──────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateWriteWord(int? lessonId)
        {
            var model = new WriteWordViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWriteWord(WriteWordViewModel model)
        {
            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.Answer = model.WriteWordAnswer.Trim();
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Voice Choices (Type 6) ───────────────────────────────────────────────
        // FIX: CorrectVoiceChoiceIndex is now 1-based (same as MultipleChoice),
        //      so the answer stored in DB is consistent across slide types.

        [HttpGet]
        public async Task<IActionResult> CreateVoiceChoices(int? lessonId)
        {
            var model = new VoiceChoicesViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVoiceChoices(VoiceChoicesViewModel model)
        {
            ValidateChoices(model.VoiceChoices, model.CorrectVoiceChoiceIndex, nameof(model.VoiceChoices));

            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.SlideContent = System.Text.Json.JsonSerializer.Serialize(new { VoiceLink = model.VoiceLink, Choices = model.VoiceChoices });
                slide.Answer       = model.CorrectVoiceChoiceIndex.ToString();
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Rich Text (Type 7) ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateRichText(int? lessonId)
        {
            var model = new RichTextViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRichText(RichTextViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.WriteWordAnswer) || model.WriteWordAnswer == "<p><br></p>")
                ModelState.AddModelError(nameof(model.WriteWordAnswer), "The expected answer cannot be empty.");

            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.Answer = model.WriteWordAnswer;
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Choices + Voice (Type 8) ─────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CreateChoicesVoices(int? lessonId)
        {
            var model = new ChoicesVoicesViewModel { LessonId = lessonId ?? 0 };
            if (lessonId.HasValue)
                model.Position = await GetNextPositionInLesson(lessonId.Value);
            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChoicesVoices(ChoicesVoicesViewModel model)
        {
            ValidateChoices(model.Choices, model.CorrectChoiceIndex, nameof(model.Choices));
            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = BuildBaseSlide(model);
                slide.SlideContent = System.Text.Json.JsonSerializer.Serialize(model.Choices);
                slide.Answer       = model.CorrectChoiceIndex.ToString();
                await SaveSlide(slide, model.LessonId);
                TempData["SuccessMessage"] = "Slide created successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex) { ModelState.AddModelError("", "An error occurred: " + ex.Message); }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Edit (base fields) ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var slide = await _context.Slides
                .Include(s => s.MasterSlideType)
                .Include(s => s.Lesson)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            var model = new SlideEditViewModel
            {
                Id               = slide.Id,
                Name             = slide.Name ?? string.Empty,
                Description      = slide.Description,
                Status           = slide.Status,
                Position         = slide.Position,
                Points           = slide.Points,
                LessonId         = slide.LessonId,
                MasterSlideTypeId = slide.MasterSlideTypeId ?? 0,
                TypeName         = slide.MasterSlideType?.Name ?? "Unknown"
            };

            await PopulateDropdowns(slide.LessonId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SlideEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) { await PopulateDropdowns(model.LessonId); return View(model); }

            try
            {
                var slide = await _context.Slides.FindAsync(id);
                if (slide == null) return NotFound();

                slide.Name        = model.Name;
                slide.Description = model.Description;
                slide.Status      = model.Status;
                slide.Position    = model.Position;
                slide.Points      = model.Points;
                slide.LessonId    = model.LessonId;

                _context.Update(slide);
                await _context.SaveChangesAsync();
                await UpdateSlidePositions(model.LessonId);

                TempData["SuccessMessage"] = "Slide updated successfully!";
                return RedirectToAction(nameof(Index), new { lessonId = model.LessonId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            await PopulateDropdowns(model.LessonId);
            return View(model);
        }

        // ─── Delete ───────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slide = await _context.Slides
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Unit)
                        .ThenInclude(u => u.Level)
                .Include(s => s.MasterSlideType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            return View(slide);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var slide = await _context.Slides.FindAsync(id);
                if (slide != null)
                {
                    var lessonId = slide.LessonId;
                    _context.Slides.Remove(slide);
                    await _context.SaveChangesAsync();
                    await UpdateSlidePositions(lessonId);

                    TempData["SuccessMessage"] = "Slide deleted successfully!";
                    return RedirectToAction(nameof(Index), new { lessonId = lessonId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting slide: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ─── Private helpers ──────────────────────────────────────────────────────

        private Slide BuildBaseSlide(SlideBaseViewModel model) => new()
        {
            Name              = model.Name,
            Description       = model.Description,
            Status            = model.Status,
            LessonId          = model.LessonId,
            MasterSlideTypeId = model.MasterSlideTypeId,
            Points            = model.Points ?? 5,
            Position          = model.Position ?? 0,
            CreatedAt         = DateTime.Now,
            UrlId             = Guid.NewGuid()
        };

        private async Task SaveSlide(Slide slide, int lessonId)
        {
            if (slide.Position == 0)
                slide.Position = await GetNextPositionInLesson(lessonId);
            _context.Add(slide);
            await _context.SaveChangesAsync();
            await UpdateSlidePositions(lessonId);
        }

        private void ValidateChoices(List<string> choices, int correctIndex, string fieldName)
        {
            if (choices.Count < 2)
                ModelState.AddModelError(fieldName, "At least 2 choices are required.");
            if (choices.Any(string.IsNullOrWhiteSpace))
                ModelState.AddModelError(fieldName, "All choices must have a value.");
            if (correctIndex < 1 || correctIndex > choices.Count)
                ModelState.AddModelError(fieldName, "Please mark one choice as correct.");
        }

        private async Task<int> GetNextPositionInLesson(int lessonId)
        {
            var max = await _context.Slides
                .Where(s => s.LessonId == lessonId)
                .MaxAsync(s => (int?)s.Position) ?? 0;
            return max + 1;
        }

        private async Task UpdateSlidePositions(int lessonId)
        {
            var slides = await _context.Slides
                .Where(s => s.LessonId == lessonId)
                .OrderBy(s => s.Position)
                .ToListAsync();
            for (int i = 0; i < slides.Count; i++)
                slides[i].Position = i + 1;
            await _context.SaveChangesAsync();
        }

        private async Task PopulateDropdowns(int? selectedLessonId = null)
        {
            ViewBag.Lessons = new SelectList(
                await _context.Lessons.OrderBy(l => l.Name).ToListAsync(),
                "Id", "Name", selectedLessonId);
        }
    }
}
