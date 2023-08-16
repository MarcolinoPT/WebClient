using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebClient.Data;
using WebClient.DTOs;

namespace WebClient.Features.ProductWhishlist
{
    [ApiController]
    [Route("api/customers/{id:guid}/wishListProducts")]
    [Produces("application/json")]
    public sealed class Create : ControllerBase
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }

        public sealed record AddProductToWhishlistRequest
        {
            public required string Name { get; init; }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDetailsResponse))]
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

        internal sealed record AddProductToWhishlistCommand : IRequest<CustomerDetailsResponse>
        {
            public required string Name { get; init; }
        }

        internal sealed class CreateProductWhishlistHandler : IRequestHandler<AddProductToWhishlistCommand, CustomerDetailsResponse>
        {
            private readonly ProductsRepository _productrsRepository;
            private readonly CustomersRepository _customersRepository;

            public CreateProductWhishlistHandler(ProductsRepository productsRepository,
                                                 CustomersRepository customersRepository)
            {
                _productrsRepository = productsRepository;
                _customersRepository = customersRepository;
            }

            public async Task<CustomerDetailsResponse> Handle(AddProductToWhishlistCommand request,
                                                              CancellationToken cancellationToken)
            {
                var product = new DTOs.ProductDto
                {
                    // We assign the product id randomly for the purpose of the challenge
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                };
                await _productrsRepository.AddProductToWhislist(
                    product: product,
                    cancellationToken: cancellationToken);
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
