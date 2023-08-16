using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebClient.Data;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
    // TODO Strict action parameter
    [Route("api/customers/{id}/wishListProducts")]
    public class Create : ControllerBase
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record ProductWhishlistRequest
        {
            public required string Name { get; init; }
        }

        [HttpPost]
        public async Task<IActionResult> HandleAsync([FromBody] ProductWhishlistRequest request,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request: new CreateProductWhishlistCommand
            {
                Name = request.Name,
            }, cancellationToken);
            return Ok(result);
        }

        internal record CreateProductWhishlistCommand : IRequest<CreateProductWishlistResponse>
        {
            public required string Name { get; init; }
        }

        internal record CreateProductWishlistResponse
        {
            public required Guid Id { get; init; }
            public required string Name { get; init; }
        }

        internal class CreateProductWhishlistHandler : IRequestHandler<CreateProductWhishlistCommand, CreateProductWishlistResponse>
        {
            private readonly ProductsRepository _repository;

            public CreateProductWhishlistHandler(ProductsRepository repository)
            {
                _repository = repository;
            }

            public async Task<CreateProductWishlistResponse> Handle(CreateProductWhishlistCommand request,
                                                                    CancellationToken cancellationToken)
            {
                var product = new DTOs.ProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                };
                await _repository.AddProductToWhislist(
                    product: product,
                    cancellationToken: cancellationToken);
                return new CreateProductWishlistResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                };
            }
        }
    }
}
