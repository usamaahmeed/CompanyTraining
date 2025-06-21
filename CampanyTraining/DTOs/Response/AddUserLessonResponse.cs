namespace CompanyTraining.DTOs.Response
{
    public class AddUserLessonResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
