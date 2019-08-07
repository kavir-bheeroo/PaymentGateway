using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway.Client.Handlers
{
    public class ValidateAuthorizationHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("You must supply an API key header called 'Authorization'")
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}