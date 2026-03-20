using LearnHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class CertificateController : Controller
    {
        private readonly LearnHubDbContext _db;

        public CertificateController(LearnHubDbContext db) => _db = db;

        // GET /Certificate/View/5  (courseId)
        public async Task<IActionResult> View(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cert = await _db.Certificates
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.CourseId == courseId);

            if (cert == null)
            {
                TempData["Error"] = "Certificate not found. Please complete and pass the quiz first.";
                return RedirectToAction("Index", "Courses");
            }

            return View(cert);
        }
    }
}
