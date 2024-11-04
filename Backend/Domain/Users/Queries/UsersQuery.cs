using Core.CQRS;
using Domain.Users.Dto;
using Domain.Users.Repositories;
using MediatR;

namespace Domain.Users.Queries;

public record UsersQuery() : IQuery<IEnumerable<UserDto>>;

internal class UsersQueryHandler(
    IUserRepository userRepository) : IRequestHandler<UsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.FindAsync(x => true, x => new UserDto(
            x.Id,
            x.Name,
            x.Surname,
            x.AreaCode,
            x.Phone,
            x.Email,
            x.Role,
            x.Addresses!.Select(a => a.Id).ToList()
        ), cancellationToken);

        return users;
    }
}