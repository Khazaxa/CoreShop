using Core.CQRS;
using Domain.Addresses.Dto;
using Domain.Addresses.Entities;
using Domain.Addresses.Repositories;
using Domain.Addresses.Services;
using Domain.Users.Repositories;
using MediatR;

namespace Domain.Addresses.Commands;

public record AddressCreateCommand(AddressDto Params, int UserId) : ICommand<int>;

internal class AddressCreateCommandHandler(
    IUserRepository userRepository,
    IAddressRepository addressRepository) : IRequestHandler<AddressCreateCommand, int>
{
    public async Task<int> Handle(AddressCreateCommand command, CancellationToken cancellationToken)
    {
        var address = new Address(
            command.Params.Street,
            command.Params.Number,
            command.Params.Apartment,
            command.Params.City,
            command.Params.PostalCode,
            command.Params.Country,
            command.UserId
        );
        
        if(!userRepository.MainAddressExistsAsync(command.UserId))
            address.MakeMain();

        var addedAddress = await addressRepository.AddAsync(address, cancellationToken);

        return addedAddress.Id;
    }
}