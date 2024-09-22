using Domain.Addresses.Commands;
using Domain.Tests.Helpers;
using FluentAssertions;

namespace Domain.Tests.Addresses.Commands
{
    [TestFixture]
    public class AddressCreateCommandHandlerTests
    {
        private TestsHelper _helper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _helper = new TestsHelper();
        }

        [Test]
        public async Task ShouldCreateAddress()
        {
            // Arrange
            var userRepository = _helper.CreateUserRepository();
            var addressRepository = _helper.CreateAddressRepository();
            var addressDto = _helper.CreateData().AddressDto;
            
            var command = new AddressCreateCommand(addressDto, 1);
            var handler = new AddressCreateCommandHandler(userRepository, addressRepository);
            var handle = handler.Handle(command, CancellationToken.None);

            // Act
            var address = await addressRepository.FindAsync(1, CancellationToken.None);
            var result = await handle;

            // Assert
            result.Should().Be(1);
            address?.Id.Should().Be(1);
            address?.Street.Should().Be("Street");
            address?.Number.Should().Be(12);
            address?.Apartment.Should().Be(12);
            address?.City.Should().Be("City");
            address?.PostalCode.Should().Be("PostalCode");
            address?.Country.Should().Be("Country");
            address?.IsMain.Should().Be(true);
                
            //await act.Should().ThrowAsync<DomainException>().Where(e => e.ErrorCode == (int)UserErrorCode);
        }
    }
}