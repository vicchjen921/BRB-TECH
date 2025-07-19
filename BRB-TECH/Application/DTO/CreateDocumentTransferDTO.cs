using Domain.Entities;

namespace Application.DTO
{
    public class CreateDocumentTransferDTO
    {
        public DocTransferType Type { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}
