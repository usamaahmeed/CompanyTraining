namespace CompanyTraining.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } =string.Empty;

        public int DurationDay { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<Subscribe> Subscribes { get; set; } = null!;
    }
}
