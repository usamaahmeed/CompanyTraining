namespace CompanyTraining.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public DateTime IssuedAt { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public int CourseId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;
        public Course Course { get; set; } = null!;

    }
}
