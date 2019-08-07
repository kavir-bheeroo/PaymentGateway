using CorrelationId;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Gateway.Common.Logging.Serilog.Enrichers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        private readonly CorrelationIdOptions _correlationIdOptions;

        public CorrelationIdEnricher(ICorrelationContextAccessor correlationContextAccessor, CorrelationIdOptions correlationIdOptions)
        {
            _correlationContextAccessor = Guard.IsNotNull(correlationContextAccessor, nameof(correlationContextAccessor));
            _correlationIdOptions = Guard.IsNotNull(correlationIdOptions, nameof(correlationIdOptions));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException("logEvent");

            string correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId;

            if (string.IsNullOrWhiteSpace(correlationId))
                return;

            var correlationIdProperty = new LogEventProperty(_correlationIdOptions.Header, new ScalarValue(correlationId));
            logEvent.AddOrUpdateProperty(correlationIdProperty);
        }
    }
}
