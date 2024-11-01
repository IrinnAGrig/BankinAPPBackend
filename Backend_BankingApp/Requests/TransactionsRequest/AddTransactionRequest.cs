using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Model;
using MediatR;

namespace BankingAppBackend.Requests.TransactionsRequest
{
    public class AddTransaction : IRequest<bool>
    {
        public TransactionDto Data { get; set; }

        public AddTransaction(TransactionDto data)
        {
            Data = data;
        }
        internal class Handler : IRequestHandler<AddTransaction, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(BankDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(AddTransaction request, CancellationToken cancellationToken)
            {
                try
                {

                         var type = Enum.TryParse<TransactionType>(request.Data.Type, out var transactionType)
                              ? transactionType
                              : throw new ArgumentException("Invalid transaction type.");

                         var info = new TransactionInfo
                         {
                              ReceiverId = request.Data.ReceiverId,
                              SenderId = request.Data.SenderId,
                              Title = request.Data.Title,
                              Subtitle = request.Data.Subtitle,
                              Emblem = request.Data.Emblem,
                              Sum = request.Data.Sum,
                              Valute = request.Data.Valute
                         };

                         var req = new Transaction
                         {
                              Id = Guid.NewGuid().ToString(),
                              Type = type,
                              Info = info,
                              Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                         };

                         // Add the new request to the Requests table
                         await _context.Transactions.AddAsync(req, cancellationToken);
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
