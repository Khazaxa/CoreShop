using Domain.Users.Entities;
using Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Authentication.Services;

internal class SystemUserContext(ShopDbContext dbContext, ILogger<SystemUserContext> logger) : ISystemUserContext
{
    private const string _systemEmail = "system@coreshop.com";

    public int UserId { get; private set; }
    public string UserEmail => _systemEmail;
    public UserRole? Role => UserRole.Admin;
    public bool IsAdmin => true;
    public bool IsAuthenticated => true;

    public async Task InitializeAsync()
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == _systemEmail);

        if (user != null)
        {
            UserId = user.Id;
            logger.LogInformation("System user initialized with ID: {UserId}", UserId);
            return;
        }

        user = new User(
            name: "Core", 
            surname: "Shop",
            areaCode: "+48",
            phone: "123456789",
            _systemEmail, 
            passwordHash: Array.Empty<byte>(),
            passwordSalt: Array.Empty<byte>(),
            UserRole.Admin
        );
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        UserId = user.Id;
        logger.LogInformation("System user created and initialized with ID: {UserId}", UserId);
    }
}