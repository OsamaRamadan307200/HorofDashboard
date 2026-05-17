using System.Diagnostics;
using HorofDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HorofDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HorofContentContext _context;

        public HomeController(ILogger<HomeController> logger, HorofContentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalLevels   = await _context.Levels.CountAsync();
            ViewBag.ActiveLevels  = await _context.Levels.CountAsync(l => l.Status);
            ViewBag.TotalUnits    = await _context.Units.CountAsync();
            ViewBag.TotalLessons  = await _context.Lessons.CountAsync();
            ViewBag.TotalSlides   = await _context.Slides.CountAsync();
            ViewBag.ActiveSlides  = await _context.Slides.CountAsync(s => s.Status);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
