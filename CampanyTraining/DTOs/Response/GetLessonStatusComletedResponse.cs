namespace CompanyTraining.DTOs.Response
{
    public class GetLessonStatusComletedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public bool isCompleted { get; set; } = false;
        public int ModuleId { get; set; }
    }
}
