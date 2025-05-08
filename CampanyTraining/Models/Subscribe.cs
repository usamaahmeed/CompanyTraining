using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Models
{

    [PrimaryKey(nameof(SessionId),nameof(ApplicationCompanyId))]
    public class Subscribe
    {
        public int PackageId { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public Package Package { get; set; } = null!;
        public string ApplicationCompanyId { get; set; } = null!;

        public string SessionId { get; set; } = string.Empty;
        public ApplicationCompany ApplicationCompany { get; set; } = null!;


    }
}
