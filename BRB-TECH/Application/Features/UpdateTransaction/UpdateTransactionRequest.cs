using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.UpdateTransaction
{
    public class UpdateTransactionRequest : IRequest<Transaction>
    {
        public UpdateTransactionDTO _updateTransaction { get; set; }

        public UpdateTransactionRequest(UpdateTransactionDTO updateTransaction)
        {
            _updateTransaction = updateTransaction;
        }
    }
}
