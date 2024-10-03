using System.Security.Cryptography;
using System.Text;
using Core.Configuration;
using Core.Database;
using Core.Exceptions;
using Domain.Users.Entities;
using Domain.Users.Enums;
using Domain.Users.Repositories;

namespace Domain.Users.Services;

internal class UserService(
    IUserRepository userRepository,
    IAppConfiguration configuration,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task ValidateIfEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (user is not null)
            throw new DomainException("User with provided email already exists", (int)UserErrorCode.EmailInUse);
    }

    public async Task SeedUserAsync()
    {
        var config = configuration.Admin;
        
        if (!await userRepository.CanConnectAsync())
        {
            Console.WriteLine("Database does not exist. Skipping user seeding.");
            return;
        }
        
        var existingUser = await userRepository.FindByEmailAsync(config.Email, CancellationToken.None);
        if (existingUser != null)
        {
            Console.WriteLine("User already exists. Skipping user creation.");
            return;
        }

        var hmac = new HMACSHA512();
        var user = new User(
            config.Name,
            config.Surname,
            config.AreaCode,
            config.Phone,
            config.Email,
            hmac.ComputeHash(Encoding.UTF8.GetBytes(config.Password)),
            hmac.Key,
            UserRole.Admin
        );

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync();
    }
}