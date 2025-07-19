using Newtonsoft.Json;

namespace Application.DTO
{
    public class AssignPermissionRequestDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("permission")]
        public string Permission { get; set; }
    }
}
