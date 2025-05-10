using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Response
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
