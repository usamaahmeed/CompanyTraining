namespace CompanyTraining.DTOs.Response
{
    public class ProfileResponse
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string .Empty;
        public string MainImg {  get; set; } = string.Empty;


    }
}
