using System.ComponentModel.DataAnnotations;

namespace LearnApiWeb.Data
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        public int UserId { get; set; }

        // Nội dung của referesh token
        public string Token { get; set; }

        // JwtId <=> Id của access token
        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        // Bị thu hồi hay chưa?
        public bool IsRevoked { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        public User User { get; set; }
    }
}