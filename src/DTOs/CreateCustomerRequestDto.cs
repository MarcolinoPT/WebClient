﻿namespace WebClient.DTOs
{
    public record CreateCustomerRequestDto
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
