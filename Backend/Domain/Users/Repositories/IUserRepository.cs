using Core.Database;
using Domain.Users.Entities;

namespace Domain.Users.Repositories;

internal interface IUserRepository : IEntityRepository<User>
{
    IQueryable<User> Query();
   
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
}