using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly LearnHubDbContext _db;

        public AdminController(LearnHubDbContext db) => _db = db;

        // Simple admin guard
        private bool IsAdmin() =>
            HttpContext.Session.GetString("IsAdmin") == "True";

        private IActionResult AdminGuard() =>
            IsAdmin() ? null! : RedirectToAction("Login", "Account");

        // GET /Admin
        public async Task<IActionResult> Index()
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var vm = new AdminDashboardViewModel
            {
                TotalCourses = await _db.Courses.CountAsync(),
                TotalUsers = await _db.Users.CountAsync(u => !u.IsAdmin),
                TotalEnrollments = await _db.Enrollments.CountAsync(),
                RecentCourses = await _db.Courses.OrderByDescending(c => c.CreatedAt).Take(5).ToListAsync(),
                RecentUsers = await _db.Users.Where(u => !u.IsAdmin).OrderByDescending(u => u.CreatedAt).Take(5).ToListAsync()
            };

            return View(vm);
        }

        // ─── Manage Courses ───────────────────────────────────────────────────
        public async Task<IActionResult> ManageCourses()
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var courses = await _db.Courses.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return View(courses);
        }

        // GET /Admin/AddCourse
        public IActionResult AddCourse()
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            return View(new Course());
        }

        // POST /Admin/AddCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(Course course)
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            if (!ModelState.IsValid) return View(course);

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Course added successfully!";
            return RedirectToAction("ManageCourses");
        }

        // GET /Admin/EditCourse/5
        public async Task<IActionResult> EditCourse(int id)
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var course = await _db.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // POST /Admin/EditCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(Course course)
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            if (!ModelState.IsValid) return View(course);

            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Course updated successfully!";
            return RedirectToAction("ManageCourses");
        }

        // POST /Admin/DeleteCourse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var course = await _db.Courses.FindAsync(id);
            if (course != null)
            {
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "Course deleted.";
            return RedirectToAction("ManageCourses");
        }

        // ─── Manage Users ─────────────────────────────────────────────────────
        public async Task<IActionResult> ManageUsers()
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var users = await _db.Users.Where(u => !u.IsAdmin).OrderByDescending(u => u.CreatedAt).ToListAsync();
            return View(users);
        }

        // POST /Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var guard = AdminGuard();
            if (guard != null) return guard;

            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "User deleted.";
            return RedirectToAction("ManageUsers");
        }
    }
}
