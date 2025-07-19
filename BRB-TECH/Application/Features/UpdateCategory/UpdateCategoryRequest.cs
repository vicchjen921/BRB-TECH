using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.UpdateCategory
{
    public class UpdateCategoryRequest : IRequest<Category>
    {
        public UpdateCategoryDTO _updateCategory { get; set; }

        public UpdateCategoryRequest(UpdateCategoryDTO updateCategory)
        {
            _updateCategory = updateCategory;
        }
    }
}
