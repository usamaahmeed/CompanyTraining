using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Response
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public bool isActive { get; set; }
        public string CategoryName { get; set; } = string.Empty;

    }

}
