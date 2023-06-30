using Microsoft.AspNetCore.Identity;

namespace LearnApiWeb.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Các thuộc tính này sẽ được thêm vào bảng
        // AspNetUsers ở CSDL
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}