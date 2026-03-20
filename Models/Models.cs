using System.ComponentModel.DataAnnotations;

namespace LearnHub.Models
{
    // ─── User ────────────────────────────────────────────────────────────────
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }

    // ─── Course ───────────────────────────────────────────────────────────────
    public class Course
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Instructor { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public double Rating { get; set; } = 4.5;

        public int StudentCount { get; set; } = 0;

        public string ImageUrl { get; set; } = "/images/course-default.jpg";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }

    // ─── Enrollment ───────────────────────────────────────────────────────────
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;
        public string PaymentMethod { get; set; } = string.Empty;

        // Navigation
        public User? User { get; set; }
        public Course? Course { get; set; }
    }

    // ─── QuizQuestion ─────────────────────────────────────────────────────────
    public class QuizQuestion
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = "A"; // A/B/C/D
        public int OrderIndex { get; set; } = 0;

        // Navigation
        public Course? Course { get; set; }
    }

    // ─── QuizAttempt ──────────────────────────────────────────────────────────
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public bool Passed { get; set; }
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
        public Course? Course { get; set; }
    }

    // ─── Certificate ──────────────────────────────────────────────────────────
    public class Certificate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
        public Course? Course { get; set; }
    }

    // ─── ContactMessage ───────────────────────────────────────────────────────
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }

    // ─── ViewModels ───────────────────────────────────────────────────────────
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class PaymentViewModel
    {
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public string CardholderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "CreditCard";
    }

    public class QuizViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public List<QuizQuestion> Questions { get; set; } = new();
        public int CurrentIndex { get; set; } = 0;
        public Dictionary<int, string> Answers { get; set; } = new();
    }

    public class HomeViewModel
    {
        public List<Course> FeaturedCourses { get; set; } = new();
        public List<CategoryStat> Categories { get; set; } = new();
    }

    public class CategoryStat
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public string Icon { get; set; } = string.Empty;
    }

    public class AdminDashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalUsers { get; set; }
        public int TotalEnrollments { get; set; }
        public List<Course> RecentCourses { get; set; } = new();
        public List<User> RecentUsers { get; set; } = new();
    }

    public class EditProfileViewModel
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [MinLength(6), DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password), Compare("NewPassword")]
        public string? ConfirmNewPassword { get; set; }
    }
}
// Append before final closing brace — see namespace
