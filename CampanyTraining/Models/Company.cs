namespace CompanyTraining.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RememberMe { get; set; }
        public string Password { get; set; }
        public string PackageId { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public ICollection<User> Users { get; set; }


    }
}
