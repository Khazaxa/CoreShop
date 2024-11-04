using Core.Database;
using Domain.Authentication.Services;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Repositories;

internal class UserRepository(
    IUnitOfWork unitOfWork, 
    ISystemUserContext systemUserContext, 
    IUserContextProvider userContextProvider,
    ShopDbContext dbContext
    ) : EntityRepositoryBase<User>(unitOfWork), IUserRepository
{
    public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user == null)
        {
            return null;
        }
        return user;
    }

    protected override IQueryable<User> GetQuery()
    {
        var query = _dbSet.Where(x => x.Id != systemUserContext.UserId);

        var currentUser = userContextProvider.HasValue ? userContextProvider.Get() : null;
        if (currentUser != null && !currentUser.IsAdmin)
            query = query.Where(x => x.Id == currentUser.UserId);

        return query;
    }
}