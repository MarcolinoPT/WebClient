using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebClient.Data;
using WebClient.DTOs;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
    [Route("api/customers/{id:guid}/wishListProducts/{productId:guid}")]
    [Produces("application/json")]
    public sealed class Delete : ControllerBase
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request: new DeleteProductWhishlistCommand(),
                                              cancellationToken);
            return Ok(result);
        }

        internal sealed record DeleteProductWhishlistCommand : IRequest<CustomerDetailsResponse>
        {
        }

        internal sealed class DeleteProductWhislistHandler : IRequestHandler<DeleteProductWhishlistCommand, CustomerDetailsResponse>
        {
            private readonly ProductsRepository _productsRepository;
            private readonly CustomersRepository _customersRepository;

            public DeleteProductWhislistHandler(ProductsRepository productsRepository,
                                                CustomersRepository customersRepository)
            {
                _productsRepository = productsRepository;
                _customersRepository = customersRepository;
            }

            public async Task<CustomerDetailsResponse> Handle(DeleteProductWhishlistCommand _,
                                                              CancellationToken cancellationToken)
            {
                await _productsRepository.DeleteProductFromWhislist(cancellationToken);
                var customerDetails = await _customersRepository.FetchCustomerDetailsAsync(cancellationToken);
                return new CustomerDetailsResponse
                {
                    Id = customerDetails.Id,
                    Name = customerDetails.Name,
                    Description = customerDetails.Description,
                    ProductsWishlisted = customerDetails.WishlistProducts.ToProductResponseArray()
                };
            }
        }
    }
}
