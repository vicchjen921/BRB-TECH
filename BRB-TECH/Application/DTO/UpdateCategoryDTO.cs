using Newtonsoft.Json;

namespace Application.DTO
{
    public class UpdateCategoryDTO
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
