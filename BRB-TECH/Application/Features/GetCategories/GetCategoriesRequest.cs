using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.GetCategories
{
    public class GetCategoriesRequest : IRequest<IReadOnlyList<Category>>
    {
        public GetCategoriesDTO _getCategory { get; set; }
        public GetCategoriesRequest(GetCategoriesDTO geteCategory)
        {
            _getCategory = geteCategory;
        }
    }
}
