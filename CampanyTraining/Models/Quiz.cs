namespace CompanyTraining.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool isPass { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<UserQuizAttempt> UserQuizzes { get; set; }

    }
}
