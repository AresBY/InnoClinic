using Polly;
using Polly.CircuitBreaker;

namespace GatewayAPI
{
    public class PollyDelegatingHandler : DelegatingHandler
    {
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public PollyDelegatingHandler()
        {
            _circuitBreakerPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(5));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _circuitBreakerPolicy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
