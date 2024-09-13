using System.Security.Cryptography;
using System.Text;
using Core.CQRS;
using Domain.Users.Dto;
using Domain.Users.Entities;
using Domain.Users.Repositories;
using Domain.Users.Services;
using MediatR;

namespace Domain.Users.Commands;

public record UserCreateCommand(UserParams Input) : ICommand<int>;

internal class UserCreateCommandHandler(
    IUserService userService,
    IUserRepository userRepository) : IRequestHandler<UserCreateCommand, int>
{
    public async Task<int> Handle(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        await userService.ValidateIfEmailExistsAsync(input.Email, cancellationToken);

        using var hmac = new HMACSHA512();
        var user = new User(
            input.Name,
            input.Surname,
            input.AreaCode,
            input.Phone,
            input.Email,
            hmac.ComputeHash(Encoding.UTF8.GetBytes(input.Password)),
            hmac.Key,
            input.Role
        );

        var addedUser = await userRepository.AddAsync(user, cancellationToken);

        return addedUser.Id;
    }
}