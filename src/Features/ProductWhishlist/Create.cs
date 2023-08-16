using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebClient.Data;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
    [Route("api/customers/{id:guid}/wishListProducts")]
    [Produces("application/json")]
    public class Create : ControllerBase
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record AddProductToWhishlistRequest
        {
            public required string Name { get; init; }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddProductToWishlistResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HandleAsync([FromBody] AddProductToWhishlistRequest request,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                request: new AddProductToWhishlistCommand
                {
                    Name = request.Name,
                }, cancellationToken);
            return Ok(result);
        }

        internal record AddProductToWhishlistCommand : IRequest<AddProductToWishlistResponse>
        {
            public required string Name { get; init; }
        }

        internal record AddProductToWishlistResponse
        {
            public required Guid Id { get; init; }
            public required string Name { get; init; }
        }

        internal class CreateProductWhishlistHandler : IRequestHandler<AddProductToWhishlistCommand, AddProductToWishlistResponse>
        {
            private readonly ProductsRepository _repository;

            public CreateProductWhishlistHandler(ProductsRepository repository)
            {
                _repository = repository;
            }

            public async Task<AddProductToWishlistResponse> Handle(AddProductToWhishlistCommand request,
                                                                   CancellationToken cancellationToken)
            {
                var product = new DTOs.ProductDto
                {
                    // We assign the product id randomly for the purpose of the challenge
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                };
                await _repository.AddProductToWhislist(
                    product: product,
                    cancellationToken: cancellationToken);
                return new AddProductToWishlistResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                };
            }
        }
    }
}
