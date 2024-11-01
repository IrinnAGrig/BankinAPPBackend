using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;

namespace BankingAppBackend.Requests.ApplicationRequest
{
    public class UpdateRequestAsync : IRequest<bool>
    {
        public RequestModel Request { get; set; }

        public UpdateRequestAsync(RequestModel Req)
        {
            Request = Req;
        }
        internal class Handler : IRequestHandler<UpdateRequestAsync, bool>
        {
            private readonly BankDbContext _context;

            public Handler(BankDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateRequestAsync request, CancellationToken cancellationToken)
            {
                try
                {
                    var req = await _context.Requests.FindAsync(request.Request.Id);

                    if (req != null)
                    {
                        req = request.Request;
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
