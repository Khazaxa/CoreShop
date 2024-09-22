using System.Security.Cryptography;
using System.Text;
using Core.CQRS;
using Domain.Authentication.Dto;
using Domain.Users.Entities;
using Domain.Users.Enums;
using Domain.Users.Repositories;
using Domain.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Domain.Authentication.Commands;

public record RegisterCommand(RegisterParams Input) : ICommand<int>;

internal class RegisterCommandHandler(
    IUserRepository userRepository,
    IUserService userService,
    IEmailSender emailSender) : IRequestHandler<RegisterCommand, int>
{
    public async Task<int> Handle(RegisterCommand command, CancellationToken cancellationToken)
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
            UserRole.Client
        );

        var registeredUser = await userRepository.AddAsync(user, cancellationToken);
        
        await emailSender.SendEmailAsync(
            registeredUser.Email,
            "Welcome to our platform",
            "Welcome to our platform, we are glad to have you here!"
        );
        
        return registeredUser.Id;
    }
}