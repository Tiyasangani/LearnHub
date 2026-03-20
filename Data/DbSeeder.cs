using LearnHub.Models;

namespace LearnHub.Data
{
    /// <summary>Seeds the database with initial courses, questions and admin user.</summary>
    public static class DbSeeder
    {
        public static void Seed(LearnHubDbContext db)
        {
            // Admin user
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    FullName = "Admin",
                    Email = "admin@learnhub.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsAdmin = true
                });
                db.SaveChanges();
            }

            // Courses
            if (!db.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new() { Title = "Introduction to Python Programming", Instructor = "Dr. Sarah Mitchell",
                        Description = "Learn Python from scratch. Master fundamentals, data structures, OOP, and build real-world projects.",
                        Category = "Programming", Price = 499, Rating = 4.8, StudentCount = 15420,
                        ImageUrl = "https://images.unsplash.com/photo-1526379095098-d400fd0bf935?w=400" },

                    new() { Title = "Complete Web Development Bootcamp", Instructor = "James Anderson",
                        Description = "HTML, CSS, JavaScript, React, Node.js — everything you need to become a full-stack developer.",
                        Category = "Web Development", Price = 699, Rating = 4.9, StudentCount = 28350,
                        ImageUrl = "https://images.unsplash.com/photo-1547658719-da2b51169166?w=400" },

                    new() { Title = "Data Science with R", Instructor = "Prof. Emily Chen",
                        Description = "Master data analysis, visualization, machine learning and statistical modelling with R.",
                        Category = "Data Science", Price = 549, Rating = 4.7, StudentCount = 9870,
                        ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=400" },

                    new() { Title = "UI/UX Design Masterclass", Instructor = "Maria Rodriguez",
                        Description = "Create beautiful, user-centred interfaces with Figma, prototyping and design thinking.",
                        Category = "Design", Price = 599, Rating = 4.8, StudentCount = 12560,
                        ImageUrl = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=400" },

                    new() { Title = "Machine Learning A-Z", Instructor = "Dr. Alex Turner",
                        Description = "Hands-on Machine Learning & Deep Learning with Python, scikit-learn and TensorFlow.",
                        Category = "Artificial Intelligence", Price = 799, Rating = 4.9, StudentCount = 21040,
                        ImageUrl = "https://images.unsplash.com/photo-1677442135703-1787eea5ce01?w=400" },

                    new() { Title = "Digital Marketing Strategy", Instructor = "Lisa Park",
                        Description = "SEO, SEM, social media, content marketing and analytics — grow any brand online.",
                        Category = "Marketing", Price = 399, Rating = 4.6, StudentCount = 18230,
                        ImageUrl = "https://images.unsplash.com/photo-1533750349088-cd871a92f312?w=400" },

                    new() { Title = "iOS App Development with Swift", Instructor = "Ryan Cooper",
                        Description = "Build real iPhone and iPad apps using Swift and SwiftUI from zero to App Store.",
                        Category = "Mobile Development", Price = 649, Rating = 4.7, StudentCount = 8940,
                        ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=400" },

                    new() { Title = "Cybersecurity Fundamentals", Instructor = "David Kim",
                        Description = "Understand network security, ethical hacking, and how to defend against modern attacks.",
                        Category = "Cybersecurity", Price = 579, Rating = 4.8, StudentCount = 14200,
                        ImageUrl = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=400" }
                };
                db.Courses.AddRange(courses);
                db.SaveChanges();
            }

            // Sample quiz questions for the first two courses
            if (!db.QuizQuestions.Any())
            {
                var pythonCourse = db.Courses.First(c => c.Category == "Programming");
                var webCourse = db.Courses.First(c => c.Category == "Web Development");

                db.QuizQuestions.AddRange(
                    // Python quiz
                    new QuizQuestion { CourseId = pythonCourse.Id, OrderIndex = 1, QuestionText = "Which keyword is used to define a function in Python?",
                        OptionA = "func", OptionB = "def", OptionC = "function", OptionD = "define", CorrectAnswer = "B" },
                    new QuizQuestion { CourseId = pythonCourse.Id, OrderIndex = 2, QuestionText = "What is the output of: print(type([]))?",
                        OptionA = "<class 'tuple'>", OptionB = "<class 'array'>", OptionC = "<class 'list'>", OptionD = "<class 'dict'>", CorrectAnswer = "C" },
                    new QuizQuestion { CourseId = pythonCourse.Id, OrderIndex = 3, QuestionText = "Which of these is a mutable data type in Python?",
                        OptionA = "tuple", OptionB = "string", OptionC = "int", OptionD = "list", CorrectAnswer = "D" },
                    new QuizQuestion { CourseId = pythonCourse.Id, OrderIndex = 4, QuestionText = "What does 'pip' stand for?",
                        OptionA = "Python Install Program", OptionB = "Pip Installs Packages", OptionC = "Python Index Packages", OptionD = "Package Index Python", CorrectAnswer = "B" },

                    // Web Dev quiz
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 1, QuestionText = "What does HTML stand for?",
                        OptionA = "HyperText Markup Language", OptionB = "High Tech Modern Language", OptionC = "Hyper Transfer Markup Language", OptionD = "Home Tool Markup Language", CorrectAnswer = "A" },
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 2, QuestionText = "Which CSS property controls the text size?",
                        OptionA = "text-style", OptionB = "font-weight", OptionC = "font-size", OptionD = "text-size", CorrectAnswer = "C" },
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 3, QuestionText = "What does the 'let' keyword do in JavaScript?",
                        OptionA = "Declares a constant", OptionB = "Imports a module", OptionC = "Defines a function", OptionD = "Declares a block-scoped variable", CorrectAnswer = "D" },
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 4, QuestionText = "Which HTTP method is typically used to submit a form?",
                        OptionA = "GET", OptionB = "POST", OptionC = "PUT", OptionD = "DELETE", CorrectAnswer = "B" },
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 5, QuestionText = "What is a CSS flexbox?",
                        OptionA = "A JavaScript library", OptionB = "A one-dimensional layout model", OptionC = "A grid system plugin", OptionD = "An HTML element", CorrectAnswer = "B" },
                    new QuizQuestion { CourseId = webCourse.Id, OrderIndex = 6, QuestionText = "Which tag makes text bold in HTML?",
                        OptionA = "<i>", OptionB = "<em>", OptionC = "<strong>", OptionD = "<mark>", CorrectAnswer = "C" }
                );
                db.SaveChanges();
            }
        }
    }
}
