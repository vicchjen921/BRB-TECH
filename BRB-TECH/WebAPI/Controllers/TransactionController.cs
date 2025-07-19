using Application.DTO;
using Application.Features.CreateTransaction;
using Application.Features.GetMonthlyBalance;
using Application.Features.GetTransactions;
using Application.Features.RemoveTransaction;
using Application.Features.UpdateTransaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "CanCreateTransaction")]
        [HttpPost("transaction")]        
        public async Task<ActionResult<Transaction>> Create(CreateTransactionDTO createTransactionRequest)
        {
            var transaction = await _mediator.Send(new CreateTransactionRequest(createTransactionRequest));
            return Ok(transaction);
        }

        [Authorize(Policy = "CanUpdateTransaction")]
        [HttpPut("requests/update")]
        public async Task<ActionResult<Transaction>> Update(UpdateTransactionDTO updateTransactionRequest)
        {
            var transaction = await _mediator.Send(new UpdateTransactionRequest(updateTransactionRequest));
            return Ok(transaction);
        }

        [Authorize(Policy = "CanReadTransactions")]
        [HttpPost("requests/transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllFiltered(GetTransactionsDTO getTransactionsRequest)
        {
            var transactions = await _mediator.Send(new GetTransactionsRequest(getTransactionsRequest));
            return Ok(transactions);
        }

        [Authorize(Policy = "CanRemoveTransaction")]
        [HttpDelete("requests/remove")]
        public async Task<ActionResult> RemoveTransaction(RemoveTransactionDTO removeTransactionRequest)
        {
            await _mediator.Send(new RemoveTransactionRequest(removeTransactionRequest));
            return Ok();
        }

        [Authorize(Policy = "CanGetMonthlyBalance")]
        [HttpPost("requests/balance")]
        public async Task<ActionResult> GetMonthlyBalance(GetMonthlyBalanceDTO getMonthlyBalanceRequest)
        {
            var balance = await _mediator.Send(new GetMonthlyBalanceRequest(getMonthlyBalanceRequest));
            return Ok(balance);
        }
    }
}
