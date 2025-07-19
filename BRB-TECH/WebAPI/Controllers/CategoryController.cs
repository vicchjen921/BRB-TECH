using Application.DTO;
using Application.Features.CreateCategory;
using Application.Features.GetCategories;
using Application.Features.RemoveCategory;
using Application.Features.UpdateCategory;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "CanCreateCategory")]
        [HttpPost("category")]
        public async Task<ActionResult<Category>> Create(CreateCategoryDTO createCategoryRequest)
        {
            var category = await _mediator.Send(new CreateCategoryRequest(createCategoryRequest));
            return Ok(category);
        }

        [Authorize(Policy = "CanUpdateCategory")]
        [HttpPut("requests/update")]
        public async Task<ActionResult<Category>> Update(UpdateCategoryDTO updateCategoryRequest)
        {
            var category = await _mediator.Send(new UpdateCategoryRequest(updateCategoryRequest));
            return Ok(category);
        }

        [Authorize(Policy = "CanReadCategories")]
        [HttpPost("requests/categories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAll(GetCategoriesDTO getCategoriesRequest)
        {
            var categories = await _mediator.Send(new GetCategoriesRequest(getCategoriesRequest));
            return Ok(categories);
        }

        [Authorize(Policy = "CanRemoveCategory")]
        [HttpDelete("requests/remove")]
        public async Task<ActionResult> RemoveCategory(RemoveCategoryDTO removeCategoryRequest)
        {
            await _mediator.Send(new RemoveCategoryRequest(removeCategoryRequest));
            return Ok();
        }
    }
}
