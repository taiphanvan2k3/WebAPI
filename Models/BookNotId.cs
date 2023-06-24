using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Dtos
{
    public class BookNotId
    {
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }
    }
}