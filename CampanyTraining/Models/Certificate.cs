namespace CompanyTraining.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public DateTime IssuedAt { get; set; }
        public int UserCourseId { get; set; }
        public UserCourse UserCourse { get; set; } = null!;

    }
}
