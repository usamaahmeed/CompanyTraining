namespace CampanyTraining.DTOs.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public bool Status { get; set; }
        public decimal Discount { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int BrandId { get; set; }
    }
}
