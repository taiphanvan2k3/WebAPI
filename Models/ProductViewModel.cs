namespace LearnApiWeb.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
    }
}