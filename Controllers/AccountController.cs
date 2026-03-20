using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly LearnHubDbContext _db;
        public AccountController(LearnHubDbContext db) => _db = db;

        // ── Login ──────────────────────────────────────────────────────────────
        [HttpGet] public IActionResult Login() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(vm);
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
            return user.IsAdmin ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Home");
        }

        // ── Register ───────────────────────────────────────────────────────────
        [HttpGet] public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (await _db.Users.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(vm);
            }
            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("IsAdmin", "False");
            return RedirectToAction("Index", "Home");
        }

        // ── Forgot Password ────────────────────────────────────────────────────
        [HttpGet] public IActionResult ForgotPassword() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            TempData["Message"] = "If that email exists, a reset link has been sent.";
            return RedirectToAction("Login");
        }

        // ── Edit Profile ───────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");
            var user = await _db.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login");
            var vm = new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.FindAsync(userId.Value);
            if (user == null) return RedirectToAction("Login");

            // Check email not taken by another user
            if (user.Email != vm.Email && await _db.Users.AnyAsync(u => u.Email == vm.Email && u.Id != userId))
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return View(vm);
            }

            user.FullName = vm.FullName;
            user.Email = vm.Email;

            // Update password only if provided
            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(vm.CurrentPassword) ||
                    !BCrypt.Net.BCrypt.Verify(vm.CurrentPassword, user.PasswordHash))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                    return View(vm);
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.NewPassword);
            }

            await _db.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", user.FullName);
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("EditProfile");
        }

        // ── Logout ─────────────────────────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
