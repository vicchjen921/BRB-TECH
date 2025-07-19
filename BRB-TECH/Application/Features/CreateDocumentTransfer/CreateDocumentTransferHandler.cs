using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.CreateDocumentTransfer
{
    public class CreateDocumentTransferHandler : IRequestHandler<CreateDocumentTransferRequest, DocumentTransfer>
    {
        private readonly IDocumentTransferRepository _documentTransferRepository;

        public CreateDocumentTransferHandler(IDocumentTransferRepository documentTransferRepository)
        {
            _documentTransferRepository = documentTransferRepository;
        }

        public async Task<DocumentTransfer> Handle(CreateDocumentTransferRequest request, CancellationToken cancellationToken)
        {
            var docTransfer = new DocumentTransfer()
            {
                CreatedAt = DateTime.UtcNow,
                DateFrom = request._createDocTransfer.DateFrom,
                DateTo = request._createDocTransfer.DateTo,
                Email = request._createDocTransfer.Email,
                UserId = request._createDocTransfer.UserId,
                Type = request._createDocTransfer.Type,
                State = DocTransferState.Opened
            };

            try
            {
                docTransfer = await _documentTransferRepository.AddAsync(docTransfer);
            }
            catch (Exception)
            {
                throw;
            }

            return docTransfer;
        }
    }
}
