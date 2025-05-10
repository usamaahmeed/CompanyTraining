using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Course> Courses { get; set; } = null!;
    }
}
