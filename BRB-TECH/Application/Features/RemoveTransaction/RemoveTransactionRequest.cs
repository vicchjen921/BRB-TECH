using Application.DTO;
using MediatR;

namespace Application.Features.RemoveTransaction
{
    public class RemoveTransactionRequest : IRequest
    {
        public RemoveTransactionDTO _removeTransaction { get; set; }

        public RemoveTransactionRequest(RemoveTransactionDTO removeTransaction)
        {
            _removeTransaction = removeTransaction;
        }
    }
}
