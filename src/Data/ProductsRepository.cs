using WebClient.DTOs;

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

        public async Task DeleteProductFromWhislist(CancellationToken cancellationToken)
        {
            var result = await _httpClient.DeleteAsync(requestUri: "/v1/customers/{id}/wishListProducts/{productId}",
                                                       cancellationToken: cancellationToken);
            _ = result.EnsureSuccessStatusCode();
        }
    }
}
