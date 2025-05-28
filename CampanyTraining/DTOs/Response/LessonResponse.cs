namespace CompanyTraining.DTOs.Response
{
    public class LessonResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public int ModuleId { get; set; }
    }
}
