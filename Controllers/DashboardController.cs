using LearnHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LearnHubDbContext _db;
        public DashboardController(LearnHubDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _db.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            var enrollments = await _db.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId.Value)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();

            var attempts = await _db.QuizAttempts
                .Include(a => a.Course)
                .Where(a => a.UserId == userId.Value)
                .OrderByDescending(a => a.AttemptedAt)
                .ToListAsync();

            var certificates = await _db.Certificates
                .Include(c => c.Course)
                .Where(c => c.UserId == userId.Value)
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.Enrollments = enrollments;
            ViewBag.Attempts = attempts;
            ViewBag.Certificates = certificates;

            return View();
        }
    }
}
