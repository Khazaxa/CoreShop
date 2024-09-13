using Domain.Addresses.Commands;
using Domain.Addresses.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Addresses;

[ApiController]
[Route("")]
public class AddressController(IMediator mediator, ILogger<AddressController> logger) : BaseController(logger)
{
    [HttpPost]
    [Route("address")]
    public async Task<int> CreateAddress(AddressDto addressParams, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        return await mediator.Send(new AddressCreateCommand(addressParams, userId), cancellationToken);
    }
}