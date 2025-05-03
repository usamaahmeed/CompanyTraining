namespace CompanyTraining.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } =string.Empty;
        public string ApplicationCompanyId { get; set; }= null!;
        public ApplicationCompany ApplicationCompany { get; set; } = null!;

        public IEnumerable<Course> Courses { get; set; } = null!;
    }
}
