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
#pragma warning disable CS8603 // Possible null reference return.
            return await result.Content.ReadFromJsonAsync<CreateCustomerResponseDto>(cancellationToken: cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<CustomerDetailsDto> FetchCustomerDetailsAsync(CancellationToken cancellationToken)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _httpClient.GetFromJsonAsync<CustomerDetailsDto>(requestUri: "/v1/customers/{id}",
                                                                          cancellationToken: cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
