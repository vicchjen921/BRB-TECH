namespace Domain.Models
{
    public class AccessToken
    {
        public string Token { get; set; }
        public long ExpiresIn { get; set; }
        public string RefreshToken  { get; set; }
        public long RefreshTokenExpiresIn { get; set; }
    }
}
