using Domain.Entities;
using Newtonsoft.Json;

namespace Application.DTO
{
    public class CreateTransactionDTO : BaseRequestDTO
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("type")]
        public TransactionType Type { get; set; }
        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
