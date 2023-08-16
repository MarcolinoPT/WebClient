using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebClient.Data;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
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
            await _mediator.Send(request: new CreateProductWhishlistCommand
            {
                Name = request.Name,
            }, cancellationToken);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        internal record CreateProductWhishlistCommand : IRequest
        {
            public required string Name { get; init; }
        }

        internal class CreateProductWhishlistHandler : IRequestHandler<CreateProductWhishlistCommand>
        {
            private readonly ProductsRepository _repository;

            public CreateProductWhishlistHandler(ProductsRepository repository)
            {
                _repository = repository;
            }

            public async Task Handle(CreateProductWhishlistCommand request,
                                     CancellationToken cancellationToken)
            {
                await _repository.AddProductToWhislist(cancellationToken: cancellationToken,
                                                       product: new DTOs.ProductDto
                                                       {
                                                           Name = request.Name,
                                                       });
            }
        }
    }
}
