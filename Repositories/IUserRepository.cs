using LearnApiWeb.Data;
using LearnApiWeb.Models;

namespace LearnApiWeb.Repositories
{
    public interface IUserRepository
    {
        public UserModel ValidateLogin(LoginModel model);

        public UserModel GetUserById(int id);

        // Viết tạm lưu token vào DB trong này
        public void SaveToken(RefreshToken token);

        public RefreshToken GetRefreshToken(TokenModel model);

        public void UpdateToken(RefreshToken token);
    }
}