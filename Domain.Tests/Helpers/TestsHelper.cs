using Domain.Addresses.Dto;
using Domain.Addresses.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Addresses.Repositories;
using Domain.Users.Entities;
using Domain.Users.Enums;
using Domain.Users.Repositories;

namespace Domain.Tests.Helpers;

public class TestsHelper
{
    private readonly DbContextOptions<ShopDbContext> _dbContextOptions = new DbContextOptionsBuilder<ShopDbContext>()
        .UseInMemoryDatabase("test")
        .Options;

    private ShopDbContext CreateDbContext()
        => new ShopDbContext(_dbContextOptions);
    
    public void AddToDatabase<TEntity>(TEntity entity) where TEntity : class
    {
        using var dbContext = CreateDbContext();
        dbContext.Add(entity);
        dbContext.SaveChanges();
    }
    
    internal UserRepository CreateUserRepository()
        => new UserRepository(new ShopDbContext(_dbContextOptions));
    
    internal AddressRepository CreateAddressRepository()
        => new AddressRepository(new ShopDbContext(_dbContextOptions));
    
    internal TestData CreateData()
    {
        var user = new User(
            "Test Name",
            "Test Surname",
            "+48",
            "123456789",
            "email@example.com",
            [],
            [],
            UserRole.Admin);
    
        var address = new Address(
            "Street",
            12,
            12,
            "City",
            "PostalCode",
            "Country",
            user.Id
        );
    
        var addressDto = new AddressDto(
            address.Id,
            address.Street,
            address.Number,
            address.Apartment,
            address.City,
            address.PostalCode,
            address.Country,
            address.IsMain
        );

        return new TestData
        {
            User = user,
            Address = address,
            AddressDto = addressDto
        };
    }
}