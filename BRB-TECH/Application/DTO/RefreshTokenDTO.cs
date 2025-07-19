using Newtonsoft.Json;

namespace Application.DTO
{
    public class RefreshTokenDTO
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
