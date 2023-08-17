namespace WebClient.Data
{
    // It might be overkill to create delegating handlers for this, but it's a good example of how to do it.
    // It's also a good example of how to use the IHttpContextAccessor to get ids from the route.
    // This approach allow the repositories to focus solely on their purpose and not on other details.

    public class CustomerIdDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerIdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            request.RequestUri = UriTokenReplacement.Replace(token: "{id}",
                                                             originalUri: request.RequestUri.OriginalString,
                                                             routeValues: _httpContextAccessor.HttpContext?.Request.RouteValues);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return base.SendAsync(request, cancellationToken);
        }
    }

    public class ProductIdDelgatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductIdDelgatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            request.RequestUri = UriTokenReplacement.Replace(token: "{productId}",
                                                             originalUri: request.RequestUri.OriginalString,
                                                             routeValues: _httpContextAccessor.HttpContext?.Request.RouteValues);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return base.SendAsync(request, cancellationToken);
        }
    }

    static class UriTokenReplacement
    {
        public static Uri Replace(string token,
                                  string originalUri,
                                  RouteValueDictionary routeValues)
        {
            var key = token.Trim('{', '}');
            string? keyValue = routeValues[key] as string;
            return new Uri(originalUri.Replace(oldValue: token,
                                              newValue: keyValue));
        }
    }
}
