namespace CompanyTraining.DTOs.Response
{
    public class CompanyResponse
    {
        public string Id { get; set; } = string.Empty; //ASPNETUSER
        public string UserName { get; set; } = string.Empty; //ASPNETUSER
        public string PackageName { get; set; } = string.Empty;  //Package
        public DateTime? SubscriptionStartDate { get; set; } //subscribe
        public DateTime? SubscriptionEndDate { get; set; } //subscribe
    }
}
