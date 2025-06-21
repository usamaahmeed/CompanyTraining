namespace CompanyTraining.DTOs.Response
{
    public class GetCertificateResponse
    {
        public int Id { get; set; }
        public DateTime IssuedAt { get; set; }
        public string ApplicationUserName { get; set; } = null!;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
    }
}
