using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LearnHubDbContext _db;

        public CoursesController(LearnHubDbContext db) => _db = db;

        // GET /Courses
        public async Task<IActionResult> Index(string? category, string? search)
        {
            var query = _db.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
                query = query.Where(c => c.Category == category);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Title.Contains(search) || c.Instructor.Contains(search));

            ViewBag.Category = category ?? "All";
            ViewBag.Search = search;
            ViewBag.Categories = new[] { "All", "Programming", "Web Development", "Data Science",
                "Design", "Artificial Intelligence", "Marketing", "Mobile Development", "Cybersecurity" };

            var courses = await query.ToListAsync();
            return View(courses);
        }

        // GET /Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course == null) return NotFound();

            // Check if current user is already enrolled
            var userId = HttpContext.Session.GetInt32("UserId");
            bool isEnrolled = userId.HasValue &&
                await _db.Enrollments.AnyAsync(e => e.UserId == userId.Value && e.CourseId == id);

            ViewBag.IsEnrolled = isEnrolled;
            return View(course);
        }
    }
}
