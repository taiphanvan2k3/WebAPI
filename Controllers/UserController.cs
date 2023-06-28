using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearnApiWeb.Data;
using LearnApiWeb.Models;
using LearnApiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LearnApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        public UserController(IUserRepository userRepository, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _userRepository = userRepository;

            // Hoặc Inject vào IConfiguration là được
            // Còn nếu muốn tự map thì injecr IOptionsMonitor vài
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("login")]
        public ActionResult Validate(LoginModel model)
        {
            User validationResult = _userRepository.ValidateLogin(model);
            if (validationResult == null)
            {
                return NotFound(new ApiRespone()
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }

            // Nếu đăng nhập đúng thì cấp token
            return Ok(new ApiRespone()
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateToken(validationResult)
            });
        }

        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Tạo mảng byte từ secretKey
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                    // roles
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                                                            SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}