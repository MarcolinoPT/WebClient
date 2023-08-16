namespace WebClient.DTOs
{
    public record ProductDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
