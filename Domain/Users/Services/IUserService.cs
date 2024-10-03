using Domain.Users.Entities;
using Domain.Users.Enums;

namespace Domain.Users.Services;

public interface IUserService
{
    Task ValidateIfEmailExistsAsync(string email, CancellationToken cancellationToken);
    Task SeedUserAsync();
    
}