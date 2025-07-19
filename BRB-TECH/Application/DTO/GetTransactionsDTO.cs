using Application.Common;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.DTO
{
    public class GetTransactionsDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("categories")]
        public List<long> Categories { get; set; }
        [JsonProperty("types")]
        public List<TransactionType> Types { get; set; }
        [JsonProperty("amountFrom")]
        public decimal AmountFrom { get; set; }
        [JsonProperty("amountTo")]
        public decimal AmountTo { get; set; }
        [JsonProperty("createdAtFrom")]
        public DateTimeOffset? CreatedAtFrom { get; set; }
        [JsonProperty("createdAtTo")]
        public DateTimeOffset? CreatedAtTo { get; set; }
        [JsonProperty("sortBy")]
        public SortBy? SortBy { get; set; }
        [JsonProperty("countPerPage")]
        public long CountPerPage { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }

    }
}
