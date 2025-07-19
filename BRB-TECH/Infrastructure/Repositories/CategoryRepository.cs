using Application.DTO;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HardCodeTest.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly BrbTechDbContext _context;

        public CategoryRepository(BrbTechDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(GetCategoriesDTO request)
        {
            return await _context.Set<Category>().Where(c => c.UserId == request.UserId && c.State == Domain.Common.EntityState.Active).ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category entity)
        {

            var item = _context.Set<Category>().FirstOrDefault(c => c.Id == entity.Id);

            if(item != null)
            {
                item.Color = entity.Color;
                item.Name = entity.Name;
                item.State = entity.State;
                await _context.SaveChangesAsync();
            }
            
            return entity;
        }
    }
}
