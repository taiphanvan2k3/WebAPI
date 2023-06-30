using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Models
{
    public class SignUpModel
    {
        // Nên có các thuộc tính trong ApplicationUser
        // Và cần Email,Password để đăng kí tài khoản
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }
    }
}