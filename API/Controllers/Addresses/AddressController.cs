using Domain.Addresses.Commands;
using Domain.Addresses.Dto;
using Domain.Addresses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Addresses;

[ApiController]
[Route("")]
public class AddressController(IMediator mediator, ILogger<AddressController> logger) : BaseController(logger)
{
    [HttpPost]
    [Route("address")]
    public async Task<int> CreateAddress(AddressParams addressParams, CancellationToken cancellationToken)
        => await mediator.Send(new AddressCreateCommand(addressParams), cancellationToken);

    [HttpGet]
    [Route("address/{addressId}")]
    public async Task<AddressDto> AddressDetails(int addressId, CancellationToken cancellationToken)
        => await mediator.Send(new AddressDetailsQuery(addressId), cancellationToken);
    
    [HttpPut]
    [Route("address/{addressId}")]
    public async Task<Unit> UpdateAddress(int addressId, AddressParams addressParams, CancellationToken cancellationToken)
        => await mediator.Send(new AddressUpdateCommand(addressId, addressParams), cancellationToken);
}