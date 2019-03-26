using AspMongo.Services.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspMongo.Handler
{
    public class SimpleAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserRepository _userRepository;

        public SimpleAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            UserRepository userRepository) 
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Auth"))
                return AuthenticateResult.Fail("Missing auth header");

            try
            {
                var authHeader = Request.Headers["Auth"].ToString();

                var credentials = authHeader.Split(":");
                var username = credentials[0];
                var password = credentials[1];

                var user = _userRepository.GetByUserNameAndPassword(username, password);

                if(user == null)
                    return AuthenticateResult.Fail("User with such username or password not found");

                var claims = new[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail($"Something went wrong: {ex.Message}");
            }
        }
    }
}
