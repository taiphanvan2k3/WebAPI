using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearnApiWeb.Data;
using LearnApiWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearnApiWeb.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        /* 
        UserManager có sẵn trong Identity
         */
        public AccountRepository(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IConfiguration configuration)
        {
            // Chèn Iconfiguration vào để lấy chuỗi SecretKey
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                                                        false, false);
            if (!result.Succeeded)
            {
                return string.Empty;
            }

            // Tạo ra các claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                
                // Jti: JWT id
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
                // Ít bữa thêm ds các roles
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey,
                                        SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var newUser = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            // Nó sẽ tự động lưu xuống AspNetUsers
            return await _userManager.CreateAsync(newUser, model.Password);
        }
    }
}