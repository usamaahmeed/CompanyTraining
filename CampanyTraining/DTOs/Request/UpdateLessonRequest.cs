namespace CompanyTraining.DTOs.Request
{
    public class UpdateLessonRequest
    {
        public string Name { get; set; } = null!;
        public int ModuleId { get; set; }
    }
}
