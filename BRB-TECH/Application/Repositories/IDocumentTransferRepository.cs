using Domain.Entities;

namespace Application.Repositories
{
    public interface IDocumentTransferRepository : IRepository<DocumentTransfer>
    {
        Task<DocumentTransfer> UpdateAsync(DocumentTransfer entity);
        Task<IReadOnlyList<DocumentTransfer>> GetOpenedAsync();
    }
}
