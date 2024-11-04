using Core.CQRS;
using Core.Exceptions;
using Domain.Users.Enums;
using Domain.Users.Repositories;
using MediatR;

namespace Domain.Users.Commands;

public record UserDeleteCommand(int Id) : ICommand<Unit>;

internal class UserDeleteCommandHandler(
    IUserRepository userRepository) : IRequestHandler<UserDeleteCommand, Unit>
{
    public async Task<Unit> Handle(UserDeleteCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindAsync(command.Id, cancellationToken)
                   ?? throw new DomainException("User not found", (int)UserErrorCode.UserNotFound);
        
        userRepository.Delete(user);
        return Unit.Value;
    }
}