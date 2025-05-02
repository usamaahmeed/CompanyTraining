namespace CompanyTraining.Models
{
    public class UserQuizAttempt
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int QuizId { get; set; }
        public int TotalQuestions { get; set; }
        public Quiz Quiz { get; set; }

        public double Score { get; set; }
        public bool Passed { get; set; }

        public ICollection<UserAnswer> Answers { get; set; }


    }
}
