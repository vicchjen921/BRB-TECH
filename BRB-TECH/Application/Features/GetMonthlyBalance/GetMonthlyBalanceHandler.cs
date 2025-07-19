using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.GetMonthlyBalance
{
    public class GetMonthlyBalanceHandler : IRequestHandler<GetMonthlyBalanceRequest, MonthlyBalance>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetMonthlyBalanceHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<MonthlyBalance> Handle(GetMonthlyBalanceRequest request, CancellationToken cancellationToken)
        {
            var balance = new MonthlyBalance();
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var userId = request._getMonthlyBalance.UserId;
                var sqlQuery = $"SELECT Id, Amount, Type, CategoryId, UserId, CreatedAt, Note, State, RowVersion " +
                               $"FROM Transactions WHERE UserId = {userId} and State = {(int)Domain.Common.EntityState.Active}" +
                               $" and CreatedAt >= '{startDate:yyyy-MM-dd}'" +
                               $" and CreatedAt < '{DateTime.Now:yyyy-MM-dd}'";
                var transactions = await _transactionRepository.GetAllFiltered(sqlQuery);

                balance.Income = transactions.Where(t => t.Type == TransactionType.TopUp).Sum(t => Math.Abs(t.Amount));
                balance.Expenses = transactions.Where(t => t.Type == TransactionType.Withdraw).Sum(t => Math.Abs(t.Amount));

                sqlQuery = $"SELECT Id, Amount, Type, CategoryId, UserId, CreatedAt, Note, State, RowVersion " +
                               $"FROM Transactions WHERE UserId = {userId} and State = {(int)Domain.Common.EntityState.Active}" +
                               $"and CreatedAt < '{startDate:yyyy-MM-dd}'";

                transactions = await _transactionRepository.GetAllFiltered(sqlQuery);

                var beginningBalance = transactions.Where(t => t.Type == TransactionType.TopUp).Sum(t => Math.Abs(t.Amount)) -
                    transactions.Where(t => t.Type == TransactionType.Withdraw).Sum(t => Math.Abs(t.Amount));

                balance.Balance = beginningBalance + balance.Income - balance.Expenses;
            }
            catch (Exception ex)
            {
                throw;
            }

            return balance;
        }
    }
}
