using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.CardRequest
{
    public class FindCardByNumberRequest : IRequest<(bool status, Card card)>
    {
        public string CardNumber { get; set; } // Assuming you're searching by card number, not ID
        public FindCardByNumberRequest(string cardNumber)
        {
            CardNumber = cardNumber;
        }

        internal class GetHandler : IRequestHandler<FindCardByNumberRequest, (bool status, Card card)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, Card card)> Handle(FindCardByNumberRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    Card card = await _context.Cards
                        .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber, cancellationToken);

                    if (card == null)
                    {
                        _logger.LogWarning("Card with number {CardNumber} not found.", request.CardNumber);
                        return (false, null); // Return false if no card is found
                    }

                    return (true, card); // Return true and the found card
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while retrieving card with number {CardNumber}: {ErrorMessage}", request.CardNumber, ex.Message);
                    return (false, null); // Return false and log the error
                }
            }
        }
    }

}
