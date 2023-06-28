using LearnApiWeb.Data;
using LearnApiWeb.Models;

namespace LearnApiWeb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;

        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }

        public User ValidateLogin(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(model.UserName)
                                                          && u.Password == model.Password);
            return user;
        }
    }
}