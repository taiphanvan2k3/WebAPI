using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LearnApiWeb.Data;
using LearnApiWeb.Helper;
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

            // Tại đây sẽ lấy được chuỗi "HelloTai"
            string test = _appSettings.Test;
        }

        [HttpGet("decode-access-token/{accessToken}")]
        public ActionResult DecodeAccessToken(string accessToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // Tự cấp token, nếu không muốn thì set là true và phải chỉ đường dẫn đến Auth0 chẳng hạn
                ValidateIssuer = false,
                ValidateAudience = false,

                // Ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                // Không kiểm tra token hết hạn, điểm này khác với ở Program.cs
                ValidateLifetime = false
            };

            var tokenVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidateParam, out var validatedToken);
            // Lấy giá trị thôn qua 
            // Decode expired time 
            var secondSinceUnixEcho = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiredDateTime = Utils.ConvertUnixTimeToDateTimeUTC(secondSinceUnixEcho);

            // Decode JWT Id
            var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            // Decode user id
            var userId = tokenVerification.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            var userName = tokenVerification.Claims.FirstOrDefault(x => x.Type == "UserName").Value;
            var name = tokenVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            var email = tokenVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var sub = tokenVerification.Claims.FirstOrDefault(x => x.Type == "Sub").Value;

            // Thay vì phải tạo ra ClaimsPrincipal từ việc validate thì ta chỉ cần làm đơn giản như sau:
            var jwtToken = new JwtSecurityToken(accessToken);
            // Lấy danh sách các Claim từ đối tượng JwtSecurityToken
            IEnumerable<Claim> claims = jwtToken.Claims;
            return Ok(claims.Select(p => new
            {
                Type = p.Type,
                Value = p.Value
            }));
        }

        [HttpPost("login")]
        public ActionResult Validate(LoginModel model)
        {
            UserModel validationResult = _userRepository.ValidateLogin(model);
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

        private TokenModel GenerateToken(UserModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Tạo mảng byte từ secretKey
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Name, user.Name),
        
                    // new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),

                    new Claim("Sub", user.Email),

                    // Jti: JWT Id
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                                                            SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            string refereshToken = GenerateRefreshToken();

            // Lưu refresh token vào database
            var refereshTokenEntity = new RefreshToken()
            {
                // Id này chẳng qua là làm khóa chính thôi
                Id = Guid.NewGuid(),
                UserId = user.Id,

                // Khi lấy token.Id thì nó sẽ lấy Claim Jti mình đã add ở trên
                JwtId = token.Id,
                Token = refereshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            _userRepository.SaveToken(refereshTokenEntity);

            return new TokenModel()
            {
                AccessToken = accessToken,
                RefereshToken = refereshToken
            };
        }

        private string GenerateRefreshToken()
        {
            // Trả về mảng byte gồm 32 phần tử được random cho nhanh 
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("renew-token")]
        public ActionResult RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Tạo mảng byte từ secretKey
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // Tự cấp token, nếu không muốn thì set là true và phải chỉ đường dẫn đến Auth0 chẳng hạn
                ValidateIssuer = false,
                ValidateAudience = false,

                // Ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                // Không kiểm tra token hết hạn, điểm này khác với ở Program.cs
                ValidateLifetime = false
            };

            // Kiểm tra xem access token còn hợp lệ không
            try
            {
                // Check 1: kiểm tra access token có đúng format không?
                var tokenVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                // check 2: check thuật toán
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return BadRequest(new ApiRespone()
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                // check 3: kiểm tra access token đã hết hạn chưa. Chỉ tạo mới token nếu chưa hết hạn
                // Nó sẽ trả về số giây tính từ ngày 1/1/1970
                var secondSinceUnixEcho = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                if (Utils.ConvertUnixTimeToDateTimeUTC(secondSinceUnixEcho) > DateTime.UtcNow)
                {
                    return BadRequest(new ApiRespone()
                    {
                        Success = false,
                        Message = "Access token has not yet expired."
                    });
                }

                // check4: check referesh token exist in DB
                var storedRefereshToken = _userRepository.GetRefreshToken(model);
                if (storedRefereshToken == null)
                {
                    return BadRequest(new ApiRespone()
                    {
                        Success = false,
                        Message = "Referesh token doesn't exist."
                    });
                }

                // check 5: check refreshToken is used/revoked
                if (storedRefereshToken.IsUsed)
                {
                    return BadRequest(new ApiRespone()
                    {
                        Success = false,
                        Message = "Referesh token has been used."
                    });
                }
                if (storedRefereshToken.IsRevoked)
                {
                    return BadRequest(new ApiRespone()
                    {
                        Success = false,
                        Message = "Referesh token has been revoked."
                    });
                }

                // check 6: Check Access token id == Jwt Id in storedRefereshToken?
                var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedRefereshToken.JwtId != jti)
                {
                    return BadRequest(new ApiRespone()
                    {
                        Success = false,
                        Message = "Token doesn't match."
                    });
                }

                // Update token is used
                storedRefereshToken.IsRevoked = true;
                storedRefereshToken.IsUsed = true;
                _userRepository.UpdateToken(storedRefereshToken);

                // Create new access token
                var user = _userRepository.GetUserById(storedRefereshToken.UserId);
                var token = GenerateToken(user);

                return Ok(new ApiRespone()
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch
            {
                return BadRequest(new ApiRespone()
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }
    }
}