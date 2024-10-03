using Core.CQRS;
using Core.Database;
using Domain.Addresses.Dto;
using Domain.Addresses.Entities;
using Domain.Addresses.Repositories;
using Domain.Authentication.Services;
using Domain.Users.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Addresses.Commands;

public record AddressCreateCommand(AddressParams Params) : ICommand<int>;

internal class AddressCreateCommandHandler(
    IUserRepository userRepository,
    IAddressRepository addressRepository,
    IUserContextProvider userContext,
    IUnitOfWork unitOfWork,
    ILogger<AddressCreateCommandHandler> logger) : IRequestHandler<AddressCreateCommand, int>
{
    public async Task<int> Handle(AddressCreateCommand command, CancellationToken cancellationToken)
    {
        var userEmail = userContext.Get().UserEmail;
        
        var user = await userRepository.FindByEmailAsync(userEmail, cancellationToken);

        var address = new Address(
            command.Params.Street,
            command.Params.Number,
            command.Params.Apartment,
            command.Params.City,
            command.Params.PostalCode,
            command.Params.Country,
           user.Id
        );
        
        var containsMainAddress = user.Addresses.Any(a => a.IsMain);
        if (!containsMainAddress)
            address.MakeMain();
        
        addressRepository.Add(address);
        logger.LogInformation("Address added to repository");

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Changes saved to database");

        return address.Id;
    }
}