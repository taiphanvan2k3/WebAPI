using LearnApiWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace LearnApiWeb.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);

        // Trả về chuỗi token
        public Task<string> SignInAsync(SignInModel model);
    }
}