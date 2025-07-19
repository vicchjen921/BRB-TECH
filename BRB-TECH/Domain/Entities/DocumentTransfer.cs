using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DocumentTransfer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DocTransferType Type { get; set; }
        public DocTransferState State { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public enum DocTransferType
    {
        PopularExpenses,
        Tendency
    }

    public enum DocTransferState
    {
        Opened,
        InProcess,
        Failed,
        Completed
    }
}
