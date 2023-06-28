using LearnApiWeb.Data;
using LearnApiWeb.Models;

namespace LearnApiWeb.Repositories
{
    public interface IUserRepository
    {
        public User ValidateLogin(LoginModel model);
    }
}