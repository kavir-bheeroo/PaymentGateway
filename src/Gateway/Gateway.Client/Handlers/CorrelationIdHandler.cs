using CorrelationId;
using Gateway.Common;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway.Client.Handlers
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        private readonly CorrelationIdOptions _correlationIdOptions;

        public CorrelationIdHandler(ICorrelationContextAccessor correlationContextAccessor, IOptions<CorrelationIdOptions> correlationIdOptions)
        {
            _correlationContextAccessor = Guard.IsNotNull(correlationContextAccessor, nameof(correlationContextAccessor));
            _correlationIdOptions = Guard.IsNotNull(correlationIdOptions, nameof(correlationIdOptions)).Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();
            request.Headers.Add(_correlationIdOptions.Header, correlationId);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}