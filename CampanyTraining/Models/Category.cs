namespace CompanyTraining.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; }= null!;
        public ApplicationUser ApplicationCompany { get; set; } = null!;
        public IEnumerable<Course> Courses { get; set; } = null!;
    }
}
