using Newtonsoft.Json;

namespace Application.DTO
{
    public class GetCategoriesDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
