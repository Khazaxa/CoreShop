using Domain.Users.Entities;

namespace Domain.Users.Repositories;

internal interface IUserRepository
{
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
}