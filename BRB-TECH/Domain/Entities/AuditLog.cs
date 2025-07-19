using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AuditLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public Action Action { get; set; }
        public string EntityName { get; set; }
        public long EntityId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public enum Action
    {
        Create,
        Read,
        Update,
        Remove
    }
}
