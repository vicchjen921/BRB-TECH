using Domain.Entities;
using Newtonsoft.Json;

namespace Application.DTO
{
    public class RemoveCategoryDTO
    {
        [JsonProperty("category")]
        public Category Category { get; set; }
    }
}
