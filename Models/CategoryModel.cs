using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Tên danh mục vượt quá số lượng cho phép")]
        public string Name { get; set; } = null!;
    }
}