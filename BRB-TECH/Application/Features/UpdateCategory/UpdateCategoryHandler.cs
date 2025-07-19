using Application.Repositories;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository, IAuditLogRepository auditLogRepository)
        {
            _categoryRepository = categoryRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Category> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = new Category()
            {
                Id = request._updateCategory.Id,
                Name = request._updateCategory.Name,
                Color = request._updateCategory.Color,
                UserId = request._updateCategory.UserId
            };

            try
            {
                var oldValue = await _categoryRepository.GetAsync(category.Id);
                category = await _categoryRepository.UpdateAsync(category);

                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Update,
                    CreatedAt = DateTime.UtcNow,
                    NewValue = JsonConvert.SerializeObject(category),
                    UserId = request._updateCategory.UserId,
                    EntityId = category.Id,
                    EntityName = "Category",
                    OldValue = oldValue != null ? JsonConvert.SerializeObject(oldValue) : "",
                };

                await _auditLogRepository.AddAsync(log);
            }
            catch (Exception)
            {
                throw;
            }

            return category;
        }
    }
}
