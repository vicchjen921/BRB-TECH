using Application.DTO;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateAsync(Category entity);
        Task<IReadOnlyList<Category>> GetAllAsync(GetCategoriesDTO request);
    }
}
