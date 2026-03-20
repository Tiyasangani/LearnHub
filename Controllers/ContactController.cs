using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Controllers
{
    public class ContactController : Controller
    {
        private readonly LearnHubDbContext _db;

        public ContactController(LearnHubDbContext db) => _db = db;

        // GET /Contact
        public IActionResult Index() => View();

        // POST /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessage msg)
        {
            if (!ModelState.IsValid) return View(msg);

            _db.ContactMessages.Add(msg);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Your message has been sent. We'll get back to you soon!";
            return RedirectToAction("Index");
        }
    }
}
