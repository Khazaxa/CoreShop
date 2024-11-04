using Core.CQRS;
using Core.Exceptions;
using Domain.Authentication.Dto;
using Domain.Authentication.Enums;
using Domain.Authentication.Services;
using Domain.Users.Repositories;

namespace Domain.Authentication.Commands;

public record LoginCommand(LoginParams Input) : ICommand<LoginResponseDto>;

internal class LoginCommandHandler(
    IUserRepository userRepository, 
    IAuthenticationService authService
) : ICommandHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = (await userRepository.FindByEmailAsync(request.Input.Email, cancellationToken))
                   ?? throw new DomainException("User or password is incorrect",
                       (int)AuthenticationErrorCode.UserOrPasswordIncorrect);

        var hash = authService.ComputePasswordHash(request.Input.Password, user.PasswordSalt);
        if (!hash.SequenceEqual(user.PasswordHash))
            throw new DomainException("User or password is incorrect",
                (int)AuthenticationErrorCode.UserOrPasswordIncorrect);

        var token = authService.GenerateToken(user.Email, user.Role, user.Id);

        return new LoginResponseDto(
            user.Id,
            user.Email,
            user.Name,
            user.Surname,
            user.Role,
            token
        );
    }
}