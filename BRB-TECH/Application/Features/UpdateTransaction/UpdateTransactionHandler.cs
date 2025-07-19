using Application.Repositories;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.UpdateTransaction
{
    public  class UpdateTransactionHandler : IRequestHandler<UpdateTransactionRequest, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public UpdateTransactionHandler(ITransactionRepository transactionRepository, IAuditLogRepository auditLogRepository)
        {
            _transactionRepository = transactionRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Transaction> Handle(UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction()
            {
                Id = request._updateTransaction.Id,
                Amount = request._updateTransaction.Amount,
                CategoryId = request._updateTransaction.CategoryId,
                Note = request._updateTransaction.Note,
                Type = request._updateTransaction.Type,
                CreatedAt = DateTimeOffset.UtcNow,
                State = Domain.Common.EntityState.Active,
                UserId = request._updateTransaction.UserId                
            };

            try
            {
                var oldValue = await _transactionRepository.GetAsync(transaction.Id);
                transaction = await _transactionRepository.UpdateAsync(transaction);

                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Update,
                    CreatedAt = DateTime.UtcNow,
                    NewValue = JsonConvert.SerializeObject(transaction),
                    UserId = request._updateTransaction.UserId,
                    EntityId = transaction.Id,
                    EntityName = "Transaction",
                    OldValue = oldValue != null ? JsonConvert.SerializeObject(oldValue) : ""
                };

                await _auditLogRepository.AddAsync(log);
            }
            catch (Exception)
            {
                throw;
            }

            return transaction;
        }
    }
}
