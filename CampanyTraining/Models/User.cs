namespace CompanyTraining.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RememberMe { get; set; }
        public string Password { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<UserQuizAttempt> UserQuizzes { get; set; }

    }
}
