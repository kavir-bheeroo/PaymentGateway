using Gateway.Common;
using Gateway.Contracts.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gateway.Host.Authentication
{
    /// <summary>
    /// Handler used for authentication using the secret key - https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
    /// </summary>
    public class SecretKeyAuthenticationHandler : AuthenticationHandler<SecretKeyAuthenticationOptions>
    {
        private const string AuthorizationHeaderName = "Authorization";
        private const string AuthenticationSchemeName = SecretKeyAuthenticationDefaults.AuthenticationScheme;
        private readonly IMerchantService _merchantService;

        public SecretKeyAuthenticationHandler(
            IOptionsMonitor<SecretKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMerchantService merchantService)
            : base(options, logger, encoder, clock)
        {
            _merchantService = Guard.IsNotNull(merchantService, nameof(merchantService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            var secretKey = Request.Headers[AuthorizationHeaderName];

            var merchant = await _merchantService.GetMerchantModelBySecretKeyAsync(secretKey);

            if (merchant == null)
            {
                return AuthenticateResult.Fail("Invalid secret key");
            }

            var claims = new[]
            {
                new Claim(Common.Extensions.ClaimTypes.MerchantId, merchant.Id.ToString()),
                new Claim(Common.Extensions.ClaimTypes.MerchantName, merchant.Name),
                new Claim(Common.Extensions.ClaimTypes.SecretKey, merchant.SecretKey)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationSchemeName));
            var authenticationTicket = new AuthenticationTicket(principal, AuthenticationSchemeName);

            return AuthenticateResult.Success(authenticationTicket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = SecretKeyAuthenticationDefaults.AuthenticationScheme;
            await base.HandleChallengeAsync(properties);
        }
    }
}