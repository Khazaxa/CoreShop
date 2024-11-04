using Domain.Users.Commands;
using Domain.Users.Dto;
using Domain.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[ApiController]
[Route("")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    public async Task<int> CreateUser(UserParams userParams, CancellationToken cancellationToken)
        => await mediator.Send(new UserCreateCommand(userParams), cancellationToken);
    
    [HttpGet]
    [Route("users")]
    public async Task<IEnumerable<UserDto>> GetUsers(CancellationToken cancellationToken)
        => await mediator.Send(new UsersQuery(), cancellationToken);
    
    [HttpPut]
    [Route("update/{id}")]
    public async Task<Unit> UpdateUser(int id, UserParams userParams, CancellationToken cancellationToken)
        => await mediator.Send(new UserUpdateCommand(id, userParams), cancellationToken);
    
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<Unit> DeleteUser(int id, CancellationToken cancellationToken)
        => await mediator.Send(new UserDeleteCommand(id), cancellationToken);
}