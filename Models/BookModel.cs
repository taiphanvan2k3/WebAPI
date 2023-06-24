using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Dtos
{
    public class BookModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Số lượng vượt quá")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }
    }
}