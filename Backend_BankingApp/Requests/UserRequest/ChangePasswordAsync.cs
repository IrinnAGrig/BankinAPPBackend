using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.ApplicationRequest;
using BankingAppBackend.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BankingAppBackend.Requests.UserRequest
{
    public class ChangePasswordAsync : IRequest<bool>
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordAsync(string id, string oldPassword, string newPassword)
        {
            Id = id;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        internal class Handler : IRequestHandler<ChangePasswordAsync, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(BankDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(ChangePasswordAsync request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
                {
                    _logger.LogWarning("Invalid password change request. User ID or passwords are null or empty.");
                    return false;
                }

                try
                {
                    var user = await _context.Users.FindAsync(request.Id);
                    if (user == null)
                    {
                        _logger.LogWarning("User not found with ID: {UserId}", request.Id);
                        return false;
                    }

                    var verifyResult = Hasher.VerifyPassword(user.Email, request.OldPassword, user.PasswordHash);
                    if (verifyResult == PasswordVerificationResult.Success)
                    {
                        user.PasswordHash = Hasher.HashPassword(user.Email, request.NewPassword);
                        await _context.SaveChangesAsync(cancellationToken); 
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Old password verification failed for user: {UserId}", request.Id);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while changing password for user {UserId}: {ErrorMessage}", request.Id, ex.Message);
                    return false;
                }
            }
        }
    }

}
