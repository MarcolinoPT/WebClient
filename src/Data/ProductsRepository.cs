﻿using WebClient.DTOs;

namespace WebClient.Data
{
    public class ProductsRepository
    {
        private readonly HttpClient _httpClient;

        public ProductsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddProductToWhislist(ProductDto product,
                                               CancellationToken cancellationToken)
        {
            var result = await _httpClient.PostAsJsonAsync(requestUri: "/v1/customers/{id}/wishListProducts",
                                                           value: product,
                                                           cancellationToken: cancellationToken);
            _ = result.EnsureSuccessStatusCode();
        }
    }

    // It might be overkill to create a delegating handler for this, but it's a good example of how to do it.
    // It's also a good example of how to use the IHttpContextAccessor to get the customer id from the route.
    // This approach allow the repository to focus solely on the produc and not on the customer details.

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
            Guid customerId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues["id"]
                                         as string);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            request.RequestUri = new Uri(request.RequestUri.OriginalString.Replace(oldValue: "{id}",
                                                                                   newValue: customerId.ToString()));
            return base.SendAsync(request, cancellationToken);
        }
    }
}