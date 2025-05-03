namespace CompanyTraining.Models
{
    public class Subscribe
    {
        public int Id { get; set; }
        public int PackageId { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public Package Package { get; set; } = null!;
        public string ApplicationCompanyId { get; set; } = null!;
        public ApplicationCompany ApplicationCompany { get; set; } = null!;


    }
}
