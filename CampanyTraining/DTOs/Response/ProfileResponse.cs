namespace CompanyTraining.DTOs.Response
{
    public class ProfileResponse
    {
        public string Id { get; set; } = null!;
        public string CompanyName { get; set; } = null!;

        public string Address { get; set; } = string.Empty;

        public string MainImg {  get; set; } = string.Empty;
        public string CoverImg { get; set; } = string.Empty;


    }
}
