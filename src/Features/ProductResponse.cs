namespace WebClient.Features
{
    public sealed record ProductResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
