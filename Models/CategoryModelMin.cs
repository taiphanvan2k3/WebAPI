using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Models
{
    public class CategoryModelMin
    {
        [MaxLength(50, ErrorMessage = "Tên danh mục vượt quá số lượng cho phép kìa Tài.")]
        public string Name { get; set; } = null!;
    }
}