using Domain.Entities;

namespace Application.DTO
{
    public class UpdateTransactionDTO
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public long CategoryId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Note { get; set; }
    }
}
