using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.ApplicationRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : Controller
    {
        private readonly IMediator _mediator;

        public RequestsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        // POST: api/requests
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] RequestDto request)
        {

            var result = await _mediator.Send( new AddTransactionRequest(request));
            return Ok(result);
        }

        //// PUT: api/requests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(string id, [FromBody] RequestModel request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateRequestAsync(request));
            return Ok(result);
        }

        //// GET: api/requests/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRequestsByIdUser(string userId)
        {
            var (status, requestList) = await _mediator.Send(new GetRequestsBySenderIdAsync(userId));

            if (!status)
            {
                return BadRequest("Unable to retrieve requests.");
            }

            return Ok(requestList.AsEnumerable().Reverse());
        }


        //// GET: api/requests/user/{userId}/opened
        [HttpGet("user/{userId}/opened")]
        public async Task<IActionResult> GetNumberOpenedRequests(string userId)
        {
            var (status, requestList) = await _mediator.Send(new GetRequestsBySenderIdAsync(userId));

            if (!status)
            {
                return BadRequest("Unable to retrieve requests.");
            }

            var openedRequestsCount = requestList.Count(r => !r.Closed);

            return Ok(openedRequestsCount);
        }

    }
}
