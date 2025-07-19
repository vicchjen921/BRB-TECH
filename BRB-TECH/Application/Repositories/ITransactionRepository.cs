using Domain.Entities;

namespace Application.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction> UpdateAsync(Transaction entity);
        Task<IEnumerable<Transaction>> GetAllFiltered(string sqlQuery);
        Task<IEnumerable<PopularExpense>> GetPopularExpenses(string userId, DateTimeOffset? dateFrom, DateTimeOffset? dateTo);
        Task<IEnumerable<Tendency>> GetTendency(string userId, DateTimeOffset? dateFrom, DateTimeOffset? dateTo);
    }
}
