using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.CreateDocumentTransfer
{
    public class CreateDocumentTransferRequest : IRequest<DocumentTransfer>
    {
        public CreateDocumentTransferDTO _createDocTransfer { get; set; }

        public CreateDocumentTransferRequest(CreateDocumentTransferDTO createDocTransfer)
        {
            _createDocTransfer = createDocTransfer;
        }
    }
}
