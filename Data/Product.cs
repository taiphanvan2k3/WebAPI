namespace LearnApiWeb.Data
{
    public class Product
    {
        public Product()
        {
            DonHangChiTiets = new List<DonHangChiTiet>();
        }
    
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string? Description { get; set; }

        public double Sales { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        
        public virtual List<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}