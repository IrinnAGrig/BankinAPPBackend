using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.CardRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.ApplicationRequest
{
    public class AddTransactionRequest : IRequest<bool>
    {
        public RequestDto Request { get; set; }

        public AddTransactionRequest(RequestDto req)
        {
            Request = req;
        }
        internal class Handler : IRequestHandler<AddTransactionRequest, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(BankDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(AddTransactionRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    // Check if both SenderId and ReceiverId exist in the Users table
                    var senderExists = await _context.Users.AnyAsync(u => u.Id == request.Request.SenderId, cancellationToken);
                    var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.Request.ReceiverId, cancellationToken);

                    // If either SenderId or ReceiverId does not exist, return false
                    if (!senderExists || !receiverExists)
                    {
                        _logger.LogError("Sender or Receiver not found in the Users table.");
                        return false;
                    }

                    var req = new RequestModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = request.Request.Name,
                        ReceiverId = request.Request.ReceiverId,
                        SenderId = request.Request.SenderId,
                        Amount = request.Request.Amount,
                        Valute = request.Request.Valute,
                        DueDate = request.Request.DueDate,
                        Phone = request.Request.Phone,
                        Email = request.Request.Email,
                        DateSent = (DateTime.Now).ToString(), // Use DateTime.Now for local time
                        Closed = false,
                        Status = request.Request.Status
                    };

                    // Add the new request to the Requests table
                    await _context.Requests.AddAsync(req, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while adding the transaction: {ErrorMessage}", ex.Message);
                    return false; // Return false in case of any error
                }
            }
        }

    }
}
