using Application.Repositories;
using Dapper;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly BrbTechDbContext _context;
        private readonly DbConnection _sqlConnection;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(ILogger<TransactionRepository> logger, BrbTechDbContext context) : base(context)
        {
            _logger = logger;
            _context = context;
            _sqlConnection = _context.Database.GetDbConnection();
        }

        public async Task<Transaction> UpdateAsync(Transaction entity)
        {
            try
            {
                var item = _context.Set<Transaction>().FirstOrDefault(c => c.Id == entity.Id);

                if (item != null)
                {
                    item.Amount = entity.Amount;
                    item.Type = entity.Type;
                    item.CategoryId = entity.CategoryId;
                    item.Note = entity.Note;
                    item.State = entity.State;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Transaction)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = await entry.GetDatabaseValuesAsync();

                        if (databaseValues == null)
                        {
                            _logger.LogWarning($"Transaction update: object with id {entity.Id} was removed");
                        }
                        else
                        {
                            entry.OriginalValues.SetValues(databaseValues); 
                            entry.CurrentValues.SetValues(proposedValues); 

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Can not update a transaction");
                throw;
            }

            return entity;
        }

        public async Task<IEnumerable<Transaction>> GetAllFiltered(string sqlQuery)
        {
            return await _sqlConnection.QueryAsync<Transaction>(sqlQuery);
        }

        public async Task<IEnumerable<PopularExpense>> GetPopularExpenses(string userId, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
        {
            var sqlQuery = $"SELECT c.Name, sum(Amount) Total FROM Transactions t" +
                $" inner join [BrbTech].[dbo].[Categories] c on t.CategoryId = c.Id" +
                $" WHERE t.UserId = '{userId}' and t.State = {(int)Domain.Common.EntityState.Active} and t.Type =0";
            try
            {
                if (dateFrom != null)
                {
                    sqlQuery += $" and CreatedAt >= '{dateFrom:yyyy-MM-dd}'";
                }

                if (dateTo != null)
                {
                    sqlQuery += $" and CreatedAt < '{dateTo:yyyy-MM-dd}'";
                }

                sqlQuery += $" group by c.Name";
                sqlQuery += $" order by Total desc";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Can not get popular expenses");
                throw;
            }

            return await _sqlConnection.QueryAsync<PopularExpense>(sqlQuery);
        }

        public async Task<IEnumerable<Tendency>> GetTendency(string userId, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
        {
            var sqlQuery = $"SELECT FORMAT(CreatedAt, 'yyyy-MM') AS Month," +
                $" SUM(CASE WHEN Type = 0 THEN Amount ELSE 0 END) AS Income," +
                $" SUM(CASE WHEN Type = 1 THEN Amount ELSE 0 END) AS Expense FROM Transactions" +
                $" WHERE UserId = '{userId}' and State = {(int)Domain.Common.EntityState.Active}";
            try
            {
                if (dateFrom != null)
                {
                    sqlQuery += $" and CreatedAt >= '{dateFrom:yyyy-MM-dd}'";
                }

                if (dateTo != null)
                {
                    sqlQuery += $" and CreatedAt < '{dateTo:yyyy-MM-dd}'";
                }

                sqlQuery += $" group by FORMAT(CreatedAt, 'yyyy-MM')";
                sqlQuery += $" order by Month";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Can not get tendency");
                throw;
            }

            return await _sqlConnection.QueryAsync<Tendency>(sqlQuery);
        }
    }
}
