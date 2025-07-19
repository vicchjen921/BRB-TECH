using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.RemoveCategory
{
    public class RemoveCategoryHandler : IRequestHandler<RemoveCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public RemoveCategoryHandler(ICategoryRepository categoryRepository, IAuditLogRepository auditLogRepository)
        {
            _categoryRepository = categoryRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task Handle(RemoveCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = new Category()
            {
                Id = request._category.Category.Id
            };

            try
            {
                category = await _categoryRepository.GetAsync(category.Id);
                category.State = Domain.Common.EntityState.Suspended;

                category = await _categoryRepository.UpdateAsync(category);
                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Remove,
                    CreatedAt = DateTime.UtcNow,
                    UserId = request._category.Category.UserId,
                    EntityId = request._category.Category.Id,
                    EntityName = "Category",
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
