namespace LearnApiWeb.Data
{
    public class User
    {
        public User()
        {
            RefreshTokens = new List<RefreshToken>();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}