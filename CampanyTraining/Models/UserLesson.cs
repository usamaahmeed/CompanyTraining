namespace CompanyTraining.Models
{
    public class UserLesson
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime ViewedAt { get; set; }=DateTime.Now;
    }
}
