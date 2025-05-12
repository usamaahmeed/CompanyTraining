using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Response
{
    public class ModuleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
    }
}
