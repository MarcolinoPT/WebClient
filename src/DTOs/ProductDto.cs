using WebClient.Features;

namespace WebClient.DTOs
{
    public record ProductDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }

    public static class ProductDtoExtensions
    {
        public static ProductResponse[] ToProductResponseArray(this IEnumerable<ProductDto> products)
        {
            return products.Select(
                x => new ProductResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToArray();
        }
    }
}
