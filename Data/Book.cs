using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Data
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        public int? MaCuaHang { get; set; }

        public virtual BookStore BookStore { get; set; }
    }
}