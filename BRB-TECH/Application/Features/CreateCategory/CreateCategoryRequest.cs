using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.CreateCategory
{
    public class CreateCategoryRequest : IRequest<Category>
    {
        public CreateCategoryDTO _createCategory { get; set; }

        public CreateCategoryRequest(CreateCategoryDTO createCategory)
        {
            _createCategory = createCategory;
        }
    }
}
