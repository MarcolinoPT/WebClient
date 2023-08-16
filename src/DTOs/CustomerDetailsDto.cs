namespace WebClient.DTOs
{
    public sealed record CustomerDetailsDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required ProductDto[] WishlistProducts { get; init; }
    }
}
