using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Controllers
{
    public class PaymentController : Controller
    {
        private readonly LearnHubDbContext _db;

        public PaymentController(LearnHubDbContext db) => _db = db;

        // GET /Payment/Checkout/5
        public async Task<IActionResult> Checkout(int courseId)
        {
            var course = await _db.Courses.FindAsync(courseId);
            if (course == null) return NotFound();

            var vm = new PaymentViewModel { CourseId = courseId, Course = course };
            return View(vm);
        }

        // POST /Payment/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(PaymentViewModel vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var course = await _db.Courses.FindAsync(vm.CourseId);
            if (course == null) return NotFound();

            // Record enrollment
            var existing = _db.Enrollments.Any(e => e.UserId == userId && e.CourseId == vm.CourseId);
            if (!existing)
            {
                _db.Enrollments.Add(new Enrollment
                {
                    UserId = userId.Value,
                    CourseId = vm.CourseId,
                    IsPaid = true,
                    PaymentMethod = vm.PaymentMethod
                });
                course.StudentCount++;
                await _db.SaveChangesAsync();
            }

            TempData["EnrolledCourseId"] = vm.CourseId;
            TempData["EnrolledCourseTitle"] = course.Title;
            return RedirectToAction("Success");
        }

        // GET /Payment/Success
        public IActionResult Success()
        {
            ViewBag.CourseId = TempData["EnrolledCourseId"];
            ViewBag.CourseTitle = TempData["EnrolledCourseTitle"];
            return View();
        }
    }
}
