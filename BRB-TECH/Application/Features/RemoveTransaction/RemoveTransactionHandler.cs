using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.RemoveTransaction
{
    public class RemoveTransactionHandler : IRequestHandler<RemoveTransactionRequest>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public RemoveTransactionHandler(ITransactionRepository transactionRepository, IAuditLogRepository auditLogRepository)
        {
            _transactionRepository = transactionRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task Handle(RemoveTransactionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = request._removeTransaction.Transaction;

                transaction.State = Domain.Common.EntityState.Suspended;

                transaction = await _transactionRepository.UpdateAsync(transaction);

                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Remove,
                    CreatedAt = DateTime.UtcNow,
                    UserId = request._removeTransaction.Transaction.UserId,
                    EntityId = request._removeTransaction.Transaction.Id,
                    EntityName = "Transaction",
                    NewValue = "",
                    OldValue = ""
                };

                await _auditLogRepository.AddAsync(log);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
