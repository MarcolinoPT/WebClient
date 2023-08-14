using WebClient.DTOs;

namespace WebClient.Data
{
    public class CustomersRepository
    {
        private readonly HttpClient _httpClient;

        public CustomersRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateCustomerResponseDto> CreateAsync(CreateCustomerRequestDto createCustomer,
                                                                 CancellationToken cancellationToken)
        {
            var result = await _httpClient.PostAsJsonAsync(requestUri: "/v1/customers",
                                                           value: createCustomer,
                                                           cancellationToken: cancellationToken);
            _ = result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<CreateCustomerResponseDto>(cancellationToken: cancellationToken);
        }
    }
}
