using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;
using MediatR;

namespace BankingAppBackend.Requests.CardRequest
{
    public class UpdateCardCommand : IRequest<bool>
    {
        public Card Card { get; set; }
        public string Id { get; set; }

        public UpdateCardCommand(string id, Card card)
        {
            Card = card;
            Id = id;
        }

        internal class UpdateCardHandler : IRequestHandler<UpdateCardCommand, bool>
        {
            private readonly BankDbContext _context;

            public UpdateCardHandler(BankDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var course = await _context.Cards.FindAsync(request.Id);

                    if (course != null)
                    {
                        course = request.Card;
                    }
                    else
                    {
                        return false;
                    }
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
