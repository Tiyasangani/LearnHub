using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class QuizController : Controller
    {
        private readonly LearnHubDbContext _db;

        public QuizController(LearnHubDbContext db) => _db = db;

        // GET /Quiz/Take/5
        public async Task<IActionResult> Take(int courseId)
        {
            var course = await _db.Courses.FindAsync(courseId);
            if (course == null) return NotFound();

            var questions = await _db.QuizQuestions
                .Where(q => q.CourseId == courseId)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();

            if (!questions.Any())
            {
                TempData["Error"] = "No quiz questions available for this course yet.";
                return RedirectToAction("Details", "Courses", new { id = courseId });
            }

            var vm = new QuizViewModel
            {
                CourseId = courseId,
                CourseTitle = course.Title,
                Questions = questions,
                CurrentIndex = 0
            };

            return View(vm);
        }

        // POST /Quiz/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int courseId, Dictionary<int, string> answers)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var questions = await _db.QuizQuestions
                .Where(q => q.CourseId == courseId)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();

            int score = 0;
            foreach (var q in questions)
            {
                if (answers.TryGetValue(q.Id, out var answer) && answer == q.CorrectAnswer)
                    score++;
            }

            bool passed = (double)score / questions.Count >= 0.6; // 60% pass mark

            // Save attempt
            _db.QuizAttempts.Add(new QuizAttempt
            {
                UserId = userId.Value,
                CourseId = courseId,
                Score = score,
                TotalQuestions = questions.Count,
                Passed = passed
            });

            // Issue certificate if passed
            if (passed)
            {
                var alreadyCertified = await _db.Certificates
                    .AnyAsync(c => c.UserId == userId.Value && c.CourseId == courseId);

                if (!alreadyCertified)
                {
                    _db.Certificates.Add(new Certificate
                    {
                        UserId = userId.Value,
                        CourseId = courseId,
                        CertificateNumber = $"LH-{userId}-{courseId}-{DateTime.UtcNow:yyyyMMdd}"
                    });
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Result", new { courseId, score, total = questions.Count, passed });
        }

        // GET /Quiz/Result
        public async Task<IActionResult> Result(int courseId, int score, int total, bool passed)
        {
            var course = await _db.Courses.FindAsync(courseId);
            ViewBag.Course = course;
            ViewBag.Score = score;
            ViewBag.Total = total;
            ViewBag.Passed = passed;
            return View();
        }
    }
}
