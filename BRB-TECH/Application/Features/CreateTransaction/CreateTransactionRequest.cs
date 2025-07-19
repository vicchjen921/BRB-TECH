using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.CreateTransaction
{
    public class CreateTransactionRequest : IRequest<Transaction>
    {
        public CreateTransactionDTO _createTransaction { get; set; }

        public CreateTransactionRequest(CreateTransactionDTO createTransaction)
        {
            _createTransaction = createTransaction;
        }
    }
}
