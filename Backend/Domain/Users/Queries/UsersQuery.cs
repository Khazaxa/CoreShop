using Core.CQRS;
using Core.Exceptions;
using Domain.Addresses.Enum;
using Domain.Users.Dto;
using Domain.Users.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Queries;

public record UsersQuery : IQuery<IEnumerable<UserDto>>;

internal class UsersQueryHandler(
    IUserRepository userRepository) : IRequestHandler<UsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.Query()
            .Include(x => x.Addresses).ToListAsync(cancellationToken);
    
        var userDtos = users.Select(x => new UserDto(
            x.Id,
            x.Name,
            x.Surname,
            x.AreaCode,
            x.Phone,
            x.Email,
            x.Role,
            (x.Addresses 
             ?? throw new DomainException("Address not found", (int)AddressErrorCode.AddressNotFound))
            .Select(a => a.Id)
            .ToList()
        ));
    
        return userDtos;
    }
}