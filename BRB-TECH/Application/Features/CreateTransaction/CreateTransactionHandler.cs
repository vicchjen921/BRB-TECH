using Application.Repositories;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.CreateTransaction
{
    public  class UpdateTransactionHandler : IRequestHandler<CreateTransactionRequest, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public UpdateTransactionHandler(ITransactionRepository transactionRepository, IAuditLogRepository auditLogRepository)
        {
            _transactionRepository = transactionRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Transaction> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction()
            {
                Amount = request._createTransaction.Amount,
                CategoryId = request._createTransaction.CategoryId,
                Note = request._createTransaction.Note,
                Type = request._createTransaction.Type,
                CreatedAt = DateTimeOffset.UtcNow,
                State = Domain.Common.EntityState.Active,
                UserId = request._createTransaction.UserId                
            };

            try
            {
                transaction = await _transactionRepository.AddAsync(transaction);

                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Create,
                    CreatedAt = DateTime.UtcNow,
                    NewValue = JsonConvert.SerializeObject(transaction),
                    OldValue = "",
                    UserId = request._createTransaction.UserId,
                    EntityId = transaction.Id,
                    EntityName = "Transaction"
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
