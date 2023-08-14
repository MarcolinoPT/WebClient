using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebClient.Data;

namespace WebClient.Features.Customer
{
    [ApiController]
    [Route("api/customers")]
    public class Create : ControllerBase
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
        public async Task<IActionResult> HandleAsync([FromBody] CreateCustomerRequest request,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request: new CreateCustomerCommand
            {
                Name = request.Name,
                Description = request.Description,

            }, cancellationToken);
            return StatusCode((int)HttpStatusCode.Created, result);
        }
        internal record NewCustomerResponse
        {
            public Guid Id { get; init; }
        }

        internal record CreateCustomerCommand : IRequest<NewCustomerResponse>
        {
            public required string Name { get; init; }
            public required string Description { get; init; }
        }

        internal class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, NewCustomerResponse>
        {
            private readonly CustomersRepository _repository;

            public CreateCustomerHandler(CustomersRepository repository)
            {
                _repository = repository;
            }

            public async Task<NewCustomerResponse> Handle(CreateCustomerCommand request,
                                                          CancellationToken cancellationToken)
            {
                var newCustomer = await _repository.CreateAsync(cancellationToken: cancellationToken,
                                                                createCustomer: new DTOs.CreateCustomerRequestDto
                                                                {
                                                                    Name = request.Name,
                                                                    Description = request.Description,
                                                                });
                return new NewCustomerResponse
                {
                    Id = newCustomer.Id
                };
            }
        }
    }
}
