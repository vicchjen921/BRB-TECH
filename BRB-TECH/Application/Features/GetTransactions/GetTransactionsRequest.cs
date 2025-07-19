using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.GetTransactions
{
    public class GetTransactionsRequest : IRequest<IEnumerable<Transaction>>
    {
        public GetTransactionsDTO _getTransaction { get; set; }

        public GetTransactionsRequest(GetTransactionsDTO getTransaction)
        {
            _getTransaction = getTransaction;
        }
    }
}
