namespace CompanyTraining.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VideoNmae { get; set; } = null!;
        public bool isCompleted { get; set; }
        
        public int ModuleId { get; set; }

        public Module Module { get; set; } = null!;

    }
}
