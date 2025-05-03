namespace CompanyTraining.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public IEnumerable<Subscribe> Subscribes { get; set; } = null!;
    }
}
