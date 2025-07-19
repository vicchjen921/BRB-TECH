using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Features.GetTransactions
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsRequest, IEnumerable<Transaction>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDistributedCache _cache;

        public GetTransactionsHandler(ITransactionRepository transactionRepository, IDistributedCache cache)
        {
            _transactionRepository = transactionRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Transaction>> Handle(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = "";
            IEnumerable<Transaction> transactions;
            var getTransactionsDTO = request._getTransaction;            
            var sqlQuery = $"SELECT Id, Amount, Type, CategoryId, UserId, CreatedAt, Note, State, RowVersion FROM Transactions WHERE UserId = '{getTransactionsDTO.UserId}' and State = {(int)Domain.Common.EntityState.Active}";
            try
            {
                if (getTransactionsDTO.Categories != null && getTransactionsDTO.Categories.Count > 0)
                {
                    sqlQuery += $" and CategoryId IN ({String.Concat(getTransactionsDTO.Categories)})";
                }

                if (getTransactionsDTO.Types != null && getTransactionsDTO.Types.Count > 0)
                {
                    sqlQuery += $" and Type IN ({String.Concat(getTransactionsDTO.Types.Select(t => (int)t))})";
                }

                if (getTransactionsDTO.AmountFrom > 0)
                {
                    sqlQuery += $" and Amount >= {getTransactionsDTO.AmountFrom}";
                }

                if (getTransactionsDTO.AmountTo > 0)
                {
                    sqlQuery += $" and Amount <= {getTransactionsDTO.AmountTo}";
                }

                if (getTransactionsDTO.CreatedAtFrom != null)
                {
                    sqlQuery += $" and CreatedAt > '{getTransactionsDTO.CreatedAtFrom:yyyy-MM-dd}'";
                }

                if (getTransactionsDTO.CreatedAtTo != null)
                {
                    sqlQuery += $" and CreatedAt < '{getTransactionsDTO.CreatedAtTo:yyyy-MM-dd}'";
                }

                var condition = "CreatedAt desc";
                if (getTransactionsDTO.SortBy != null)
                {

                    switch (getTransactionsDTO.SortBy)
                    {
                        case Common.SortBy.CategoryAsc: condition = "CategoryId"; break;
                        case Common.SortBy.CategoryDesc: condition = "CategoryId desc"; break;
                        case Common.SortBy.AmountAsc: condition = "Amount"; break;
                        case Common.SortBy.AmountDesc: condition = "Amount desc"; break;
                        case Common.SortBy.CreatedAtAsc: condition = "CreatedAt"; break;
                        case Common.SortBy.CreatedAtDesc: condition = "CreatedAt desc"; break;
                        case Common.SortBy.TypeAsc: condition = "Type"; break;
                        case Common.SortBy.TypeDesc: condition = "Type desc"; break;
                    }
                }

                sqlQuery += $" order by {condition}";

                cacheKey = $"transactions:{getTransactionsDTO.UserId}:Categories{(getTransactionsDTO.Categories != null ? String.Concat(getTransactionsDTO.Categories) : "")}:" +
                    $"Type{(getTransactionsDTO.Types != null ? String.Concat(getTransactionsDTO.Types.Select(t => (int)t)) : "")}:" +
                    $"AmountFrom{getTransactionsDTO.AmountFrom}:AmountTo{getTransactionsDTO.AmountTo}:" +
                    $"CreatedAtFrom{getTransactionsDTO.CreatedAtFrom:yyyy-MM-dd}:CreatedAtTo{getTransactionsDTO.CreatedAtTo:yyyy-MM-dd}:OrderBy{condition}:" +
                    $"CountPerPage{getTransactionsDTO.CountPerPage}:Page{getTransactionsDTO.Page}";

                var cachedTransactions = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedTransactions))
                {
                    transactions = JsonConvert.DeserializeObject<IEnumerable<Transaction>>(cachedTransactions);
                    return transactions;
                }

                if (getTransactionsDTO.CountPerPage > 0)
                {
                    sqlQuery += $" OFFSET {(getTransactionsDTO.Page - 1) * getTransactionsDTO.CountPerPage} ROWS " +
                                $"FETCH NEXT {getTransactionsDTO.CountPerPage} ROWS ONLY";
                }

                transactions = await _transactionRepository.GetAllFiltered(sqlQuery);
                var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(transactions), cacheOptions);
                return transactions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
