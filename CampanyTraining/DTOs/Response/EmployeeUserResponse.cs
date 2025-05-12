namespace CompanyTraining.DTOs.Response
{
    public class EmployeeUserResponse
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? Address;
        public string MainImg { get; set; } = null!;
        public string CompanyId { get; set; } = null!;

    }
}
