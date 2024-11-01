using BankingAppBackend.DTO;
using BankingAppBackend.Requests.ApplicationRequest;
using BankingAppBackend.Requests.TransactionsRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Invalid transaction data.");
            }

            var result = await _mediator.Send(new AddTransaction(transaction));
            if (result)
            {
                return Ok(true);
            }

            return StatusCode(500, "Failed to add transaction.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var transactions = await _mediator.Send(new GetTransactionsByUserIdRequest(userId));
            return Ok(transactions);
        }
    }
}
