using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebClient.Data;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
    [Route("api/customers/{id:guid}/wishListProducts/{productId:guid}")]
    public class Delete : ControllerBase
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request: new DeleteProductWhishlistCommand(),
                                 cancellationToken);
            return NoContent();

        }
        internal record DeleteProductWhishlistCommand : IRequest
        {
        }

        internal class DeleteProductWhislistHandler : IRequestHandler<DeleteProductWhishlistCommand>
        {
            private readonly ProductsRepository _repository;

            public DeleteProductWhislistHandler(ProductsRepository repository)
            {
                _repository = repository;
            }

            public async Task Handle(DeleteProductWhishlistCommand _,
                                     CancellationToken cancellationToken)
            {
                await _repository.DeleteProductFromWhislist(cancellationToken);
            }
        }
    }
}
