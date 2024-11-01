using BankingAppBackend.Model;
using BankingAppBackend.Requests.UserRequest;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
     
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _configuration;

        public UsersController(IMediator mediator, ILogger<UsersController> logger, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginData loginData)
        {
            if (loginData == null)
            {
                return BadRequest("Invalid login data.");
            }
            if (string.IsNullOrWhiteSpace(loginData.Email))
            {
                _logger.LogWarning("Email is null or empty.");
                return Unauthorized("Email error on sending.");
            }

            var user = await _mediator.Send( new FindByEmailAsync(loginData.Email));
            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginData.Email);
                return Unauthorized("Invalid email or password.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginData.Email);
                return Unauthorized("Invalid email or password.");
            }

            //var token = GenerateJwtToken(user);
            _logger.LogInformation("User {Email} logged in successfully.", loginData.Email);
            return Ok(user);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpData signUpData)
        {
            if (signUpData == null)
            {
                return BadRequest("Invalid signup data.");
            }

            var result = await _mediator.Send(new CreateUserAsync(signUpData));
            if (result.status)
            {
             //   var token = GenerateJwtToken(result.user); 

                _logger.LogInformation("User {Email} signed up successfully.", signUpData.Email);
                return Ok(result.user); 
            }

            _logger.LogWarning("Errors: {Errors}", signUpData.Email);
            return BadRequest();
        }


        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordData changePasswordData)
        {
            var result = await _mediator.Send(new ChangePasswordAsync(id, changePasswordData.OldPassword, changePasswordData.NewPassword));
            if (result)
            {
                _logger.LogInformation("Password changed successfully for user: {Id}", id);
                return Ok();
            }

            return BadRequest();
        }

          [HttpPost("editprofile")]
          public async Task<IActionResult> EditProfile( [FromBody] User userData)
        {
            var result = await _mediator.Send(new UpdateAsync(userData.Id, userData));
            if (result)
            {
                _logger.LogInformation("Profile updated successfully for user: {Id}", userData.Id);
                return Ok(result);
            }

            return BadRequest();
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:BankCombo"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
