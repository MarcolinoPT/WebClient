using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebClient.Data;
using WebClient.DTOs;

namespace WebClient.Features.Customer
{
    [ApiController]
    [Route("api/customers")]
    [Produces("application/json")]
    public sealed class Create : ControllerBase
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record CreateCustomerRequest
        {
            public required string Name { get; init; }
            public required string Description { get; init; }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HandleAsync([FromBody] CreateCustomerRequest request,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                request: new CreateCustomerCommand
                {
                    Name = request.Name,
                    Description = request.Description,

                }, cancellationToken);
            return StatusCode(statusCode: (int)HttpStatusCode.Created,
                              value: result);
        }

        internal record CreateCustomerCommand : IRequest<NewCustomerResponse>
        {
            public required string Name { get; init; }
            public required string Description { get; init; }
        }

        public record NewCustomerResponse
        {
            public required Guid Id { get; init; }
            public required string Name { get; init; }
            public required string Description { get; init; }
            public required ProductResponse[] ProductsWishlisted { get; init; }
        }

        public sealed record ProductResponse
        {
            public required Guid Id { get; init; }
            public required string Name { get; init; }
        }

        internal class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, NewCustomerResponse>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly CustomersRepository _repository;

            public CreateCustomerHandler(IHttpContextAccessor httpContextAccessor,
                                         CustomersRepository repository)
            {
                _httpContextAccessor = httpContextAccessor;
                _repository = repository;
            }

            public async Task<NewCustomerResponse> Handle(CreateCustomerCommand request,
                                                          CancellationToken cancellationToken)
            {
                var newCustomer = await _repository.CreateAsync(cancellationToken: cancellationToken,
                                                                createCustomer: new CreateCustomerRequestDto
                                                                {
                                                                    Name = request.Name,
                                                                    Description = request.Description,
                                                                });
                // Lets inject the customer id and use the id
                // delegating handler to maintain the usage pattern
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _httpContextAccessor.HttpContext.Request.RouteValues.Add(key: "id",
                                                                         value: newCustomer.Id.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                var customerDetails = await _repository.FetchCustomerDetailsAsync(cancellationToken);
                return new NewCustomerResponse
                {
                    Id = newCustomer.Id,
                    Description = customerDetails.Description,
                    Name = customerDetails.Name,
                    ProductsWishlisted = customerDetails.WishlistProducts.Select(
                        x => new ProductResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                        }).ToArray(),
                };
            }
        }
    }
}
