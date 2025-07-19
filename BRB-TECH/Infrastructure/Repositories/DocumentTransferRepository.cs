using Application.Repositories;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DocumentTransferRepository : Repository<DocumentTransfer>, IDocumentTransferRepository
    {
        private readonly BrbTechDbContext _context;

        public DocumentTransferRepository(BrbTechDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {

            var item = _context.Set<Category>().FirstOrDefault(c => c.Id == entity.Id);

            if (item != null)
            {
                item.Color = entity.Color;
                item.Name = entity.Name;
                item.State = entity.State;
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<IReadOnlyList<DocumentTransfer>> GetOpenedAsync()
        {
            return await _context.Set<DocumentTransfer>().Where(c => c.State == DocTransferState.Opened).ToListAsync();
        }

        public async Task<DocumentTransfer> UpdateAsync(DocumentTransfer entity)
        {
            var item = _context.Set<DocumentTransfer>().FirstOrDefault(c => c.Id == entity.Id);

            if (item != null)
            {
                item.State = entity.State;
                item.Timestamp = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
            }

            return entity;
        }
    }
}
