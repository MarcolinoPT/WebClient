namespace WebClient.Features
{
    public sealed record CustomerDetailsResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required ProductResponse[] ProductsWishlisted { get; init; }
    }
}
