using Domain.Addresses.Commands;
using Domain.Addresses.Dto;
using Domain.Addresses.Repositories;
using Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Domain.Tests.Addresses.Commands
{
    [TestFixture]
    public class AddressCreateCommandHandlerTests
    {
        [Test]
        public async Task ShouldCreateAddress()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ShopDbContext>()
                .UseInMemoryDatabase("test")
                .Options;
            var userRepository = new UserRepository(dbContext: new ShopDbContext(dbContextOptions));
            var addressRepository = new AddressRepository(dbContext: new ShopDbContext(dbContextOptions));
            var addressDto = new AddressDto(
                1,
                "Street",
                12,
                12,
                "City",
                "PostalCode",
                "Country",
                false
            );
            var command = new AddressCreateCommand(addressDto, 1);
            var handler = new AddressCreateCommandHandler(userRepository, addressRepository);
            var handle = handler.Handle(command, CancellationToken.None);
            
            // Act
            var address = await addressRepository.FindAsync(1, CancellationToken.None);
            var result = await handle;
            
            // Assert
            Assert.That(result, Is.EqualTo(1));
            if (address != null)
            {
                Assert.That(address.Id, Is.EqualTo(1));
                Assert.That(address.Street, Is.EqualTo("Street"));
                Assert.That(address.Number, Is.EqualTo(12));
                Assert.That(address.Apartment, Is.EqualTo(12));
                Assert.That(address.City, Is.EqualTo("City"));
                Assert.That(address.PostalCode, Is.EqualTo("PostalCode"));
                Assert.That(address.Country, Is.EqualTo("Country"));
                Assert.That(address.IsMain, Is.EqualTo(true));
            }
        }
    }
}