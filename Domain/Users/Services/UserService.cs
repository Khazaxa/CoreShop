using System.Security.Cryptography;
using System.Text;
using Core.Exceptions;
using Domain.Users.Entities;
using Domain.Users.Enums;
using Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Services;

internal class UserService(IUserRepository userRepository,
    ShopDbContext dbContext) : IUserService
{
    public async Task ValidateIfEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (user is not null)
            throw new DomainException("User with provided email already exists", (int)UserErrorCode.EmailInUse);
    }
    
    public async Task CreateInitialUserAsync(
        string email,
        string password,
        string name,
        string surname,
        string areaCode,
        string phone,
        CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken))
            return;

        using var hmac = new HMACSHA512();
        var user = new User(
            name,
            surname,
            areaCode,
            phone,
            email,
            passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            passwordSalt: hmac.Key,
            UserRole.Admin
        );

        await userRepository.AddAsync(user, cancellationToken);
    }
}