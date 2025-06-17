namespace CompanyTraining.DTOs.Response
{
    public class GetLessonsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public bool isCompleted { get; set; } = false;
    }
}
