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

        public UserModel GetUserById(int id)
        {
            return _context.Users.Where(u => u.Id == id)
                                .Select(u => new UserModel()
                                {
                                    Id = u.Id,
                                    UserName = u.UserName,
                                    Password = u.Password,
                                    Name = u.Name,
                                    Email = u.Email
                                })
                                .FirstOrDefault();
        }

        public UserModel ValidateLogin(LoginModel model)
        {
            var user = _context.Users.Where(u => u.UserName.Equals(model.UserName)
                                                          && u.Password == model.Password)
                                    .Select(u => new UserModel()
                                {
                                    Id = u.Id,
                                    UserName = u.UserName,
                                    Password = u.Password,
                                    Name = u.Name,
                                    Email = u.Email
                                })
                                .FirstOrDefault();
            return user;
        }

        public void SaveToken(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            _context.SaveChanges();
        }

        public RefreshToken GetRefreshToken(TokenModel model)
        {
            return _context.RefreshTokens.FirstOrDefault(p => p.Token == model.RefereshToken);
        }

        public void UpdateToken(RefreshToken token)
        {
            _context.Update(token);
            _context.SaveChanges();
        }
    }
}