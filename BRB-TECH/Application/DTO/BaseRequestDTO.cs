using Newtonsoft.Json;

namespace Application.DTO
{
    public class BaseRequestDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
