namespace CompanyTraining.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public IEnumerable<Lesson> Lessons { get; set; } = null!;
    }
}
