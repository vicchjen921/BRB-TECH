using Application.DTO;
using MediatR;

namespace Application.Features.RemoveCategory
{
    public class RemoveCategoryRequest : IRequest
    {
        public RemoveCategoryDTO _category { get; set; }

        public RemoveCategoryRequest(RemoveCategoryDTO category)
        {
            _category = category;
        }
    }
}
