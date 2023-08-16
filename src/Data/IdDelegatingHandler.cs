namespace WebClient.Data
{
    // It might be overkill to create a delegating handler for this, but it's a good example of how to do it.
    // It's also a good example of how to use the IHttpContextAccessor to get the customer id from the route.
    // This approach allow the repository to focus solely on the product and not on the customer details.

    public class IdDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            Guid customerId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues["id"]
                                         as string ?? Guid.Empty.ToString());
            // Skip at all if there is no customer id
            if (customerId == Guid.Empty)
            {
                return base.SendAsync(request, cancellationToken);
            }
            Guid productId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues["productId"] as string
                                        ?? Guid.Empty.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            request.RequestUri = new Uri(uriString: request.RequestUri.OriginalString.Replace(oldValue: "{id}",
                                                                                              newValue: customerId.ToString()));
            if (productId != Guid.Empty)
            {
                request.RequestUri = new Uri(uriString: request.RequestUri.OriginalString.Replace(oldValue: "{productId}",
                                                                                                  newValue: productId.ToString()));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return base.SendAsync(request, cancellationToken);
        }
    }
}
