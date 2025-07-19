using Domain.Common;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }        
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public long CategoryId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Note { get; set; }
        public EntityState State { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }


    public enum TransactionType
    {
        /// <summary>
        /// TopUp
        /// </summary>
        TopUp,
        /// <summary>
        /// Withdraw
        /// </summary>
        Withdraw
    }
}
