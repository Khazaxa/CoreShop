using Core.CQRS;
using Core.Exceptions;
using Domain.Users.Dto;
using Domain.Users.Enums;
using Domain.Users.Repositories;
using MediatR;

namespace Domain.Users.Commands;

public record UserUpdateCommand(int Id, UserParams Params) : ICommand<Unit>;

internal class UserUpdateCommandHandler(IUserRepository userRepository) : IRequestHandler<UserUpdateCommand, Unit>
{
    public async Task<Unit> Handle(UserUpdateCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindAsync(command.Id, cancellationToken)
            ?? throw new DomainException("User not found", (int)UserErrorCode.UserNotFound);
        
        user.Update(command.Params);
        userRepository.Update(user);
        
        return Unit.Value;
    }
}