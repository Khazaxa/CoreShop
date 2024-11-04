using Core.Exceptions;
using Domain.Authentication.Commands;
using Domain.Authentication.Dto;
using Domain.Users.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Authentication;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthenticationController(IMediator mediator) : ControllerBase
{
    [HttpPost, Route("login")]
    public Task<LoginResponseDto> Login(LoginParams loginParams, CancellationToken cancellationToken)
        => mediator.Send(new LoginCommand(loginParams), cancellationToken);
    
    [HttpPost, Route("register")]
    public Task<int> Register(RegisterParams registerParams, CancellationToken cancellationToken)
        => mediator.Send(new RegisterCommand(registerParams), cancellationToken);

}