using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;
using MediatR;

namespace BankingAppBackend.Requests.CardRequest
{
    public class AddCardRequest : IRequest<bool>
    {
        public CardDTO Card { get; set; }

        public AddCardRequest(CardDTO card)
        {
            Card = card;
        }

        internal class AddCardHandler : IRequestHandler<AddCardRequest, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<AddCardHandler> _logger;

            public AddCardHandler(BankDbContext context, ILogger<AddCardHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(AddCardRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    // Map CardDTO to Card entity
                    var card = new Card
                    {
                        Id = Guid.NewGuid().ToString(),
                        OwnerId = request.Card.OwnerId,
                        CardNumber = request.Card.CardNumber,
                        NameHolder = request.Card.NameHolder,
                        ExpiryDate = request.Card.ExpiryDate,
                        Cvv = request.Card.Cvv,
                        Type = request.Card.Type,
                        Balance = request.Card.Balance,
                        Valute = request.Card.Valute
                    };

                    // Add the new card to the database
                    await _context.Cards.AddAsync(card, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Card with number {CardNumber} added successfully.", card.CardNumber);

                    // If result > 0, the card was added successfully
                    return result > 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while adding the card: {ErrorMessage}", ex.Message);
                    return false; // Return false in case of any error
                }
            }
        }
    }

}
