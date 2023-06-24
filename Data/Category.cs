namespace LearnApiWeb.Data
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual List<Product> Products { get; set; }
    }
}