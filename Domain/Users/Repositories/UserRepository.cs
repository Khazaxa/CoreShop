using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Repositories;

internal class UserRepository(ShopDbContext dbContext) : IUserRepository
{
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
    
    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        => await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public bool MainAddressExistsAsync(int userId)
       => dbContext.Users.Any(u => u.Addresses!
                .Any(a => a.UserId == userId && a.IsMain));
}