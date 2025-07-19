using Application.DTO;
using Application.Features.CreateDocumentTransfer;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTransferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentTransferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "CanCreateDocTransfer")]
        [HttpPost("docTransfer")]
        public async Task<ActionResult<DocumentTransfer>> Create(CreateDocumentTransferDTO createDocTransferRequest)
        {
            var category = await _mediator.Send(new CreateDocumentTransferRequest(createDocTransferRequest));
            return Ok(category);
        }
    }
}
