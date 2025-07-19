using Newtonsoft.Json;

namespace Application.DTO
{
    public class CreateCategoryDTO : BaseRequestDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
