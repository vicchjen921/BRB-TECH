using Application.Repositories;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, IAuditLogRepository auditLogRepository)
        {
            _categoryRepository = categoryRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Category> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = new Category()
            {
                Name = request._createCategory.Name,
                Color = request._createCategory.Color,
                UserId = request._createCategory.UserId,
                State = Domain.Common.EntityState.Active
            };

            try
            {
               category = await _categoryRepository.AddAsync(category);

                var log = new AuditLog()
                {
                    Action = Domain.Entities.Action.Create,
                    CreatedAt = DateTime.UtcNow,
                    NewValue = JsonConvert.SerializeObject(category),
                    OldValue = "",
                    UserId = request._createCategory.UserId,
                    EntityId = category.Id,
                    EntityName = "Category"                    
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
