using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.ApplicationRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.TransactionsRequest
{
    public class GetTransactionsByUserIdRequest : IRequest<(bool status, List<Transaction> list)>
    {
        public string Id { get; set; }
        public GetTransactionsByUserIdRequest(string id)
        {
            Id = id;
        }
        internal class GetHandler : IRequestHandler<GetTransactionsByUserIdRequest, (bool status, List<Transaction>)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, List<Transaction>)> Handle(GetTransactionsByUserIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Transaction> transactions = new List<Transaction>();

                    transactions = await _context.Transactions
                            .Where(g => EF.Functions.Like(g.Info.SenderId, request.Id))
                            .ToListAsync();


                    return (true, transactions);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to create category: {title}. Error: {message}", ex.Message);
                    return (false, null);
                }
            }
        }
    }
}
