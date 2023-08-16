namespace WebClient.ConfigOptions
{
    public sealed record AppConfig
    {
        public BaseAddressOptions BaseAddress { get; init; } = new();
    }
}
