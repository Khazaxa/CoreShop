using Core.Database;
using Core.Exceptions;
using Domain.Authentication.Services;
using Domain.Users.Entities;
using Domain.Users.Enums;
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
        return await GetQuery().FirstOrDefaultAsync(x => x.Email == email, cancellationToken)
               ?? throw new DomainException("User not found", (int)UserErrorCode.UserNotFound);
    }

    protected override IQueryable<User> GetQuery()
    {
        var query = _dbSet.Where(x => x.Id != systemUserContext.UserId);

        var currentUser = userContextProvider.HasValue ? userContextProvider.Get() : null;
        if (currentUser != null && !currentUser.IsAdmin)
            query = query.Where(x => x.Id == currentUser.UserId);

        return query;
    }
    
    public async Task<bool> CanConnectAsync()
    {
        return await dbContext.Database.CanConnectAsync();
    }
}