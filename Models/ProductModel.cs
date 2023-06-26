namespace LearnApiWeb.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? CategoryName { get; set; }
    }
}