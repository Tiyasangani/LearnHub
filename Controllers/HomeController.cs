using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly LearnHubDbContext _db;

        public HomeController(LearnHubDbContext db) => _db = db;

        // GET /
        public async Task<IActionResult> Index()
        {
            var featured = await _db.Courses.OrderByDescending(c => c.StudentCount).Take(4).ToListAsync();

            var categories = new List<CategoryStat>
            {
                new() { Name = "Programming",        Count = await _db.Courses.CountAsync(c => c.Category == "Programming"),        Icon = "bi-code-slash" },
                new() { Name = "Web Development",    Count = await _db.Courses.CountAsync(c => c.Category == "Web Development"),    Icon = "bi-globe" },
                new() { Name = "Data Science",       Count = await _db.Courses.CountAsync(c => c.Category == "Data Science"),       Icon = "bi-bar-chart" },
                new() { Name = "Design",             Count = await _db.Courses.CountAsync(c => c.Category == "Design"),             Icon = "bi-palette" },
                new() { Name = "Artificial Intelligence", Count = await _db.Courses.CountAsync(c => c.Category == "Artificial Intelligence"), Icon = "bi-cpu" },
                new() { Name = "Marketing",          Count = await _db.Courses.CountAsync(c => c.Category == "Marketing"),          Icon = "bi-graph-up-arrow" },
                new() { Name = "Mobile Development", Count = await _db.Courses.CountAsync(c => c.Category == "Mobile Development"), Icon = "bi-phone" },
                new() { Name = "Cybersecurity",      Count = await _db.Courses.CountAsync(c => c.Category == "Cybersecurity"),      Icon = "bi-shield-lock" },
            };

            var vm = new HomeViewModel { FeaturedCourses = featured, Categories = categories };
            return View(vm);
        }

        // GET /Home/About
        public IActionResult About() => View();

        // GET /Home/Error
        public IActionResult Error() => View();
    }
}
