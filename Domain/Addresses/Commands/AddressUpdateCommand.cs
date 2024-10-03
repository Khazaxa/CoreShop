using Core.CQRS;
using Core.Database;
using Core.Exceptions;
using Domain.Addresses.Dto;
using Domain.Addresses.Enum;
using Domain.Addresses.Repositories;
using Domain.Authentication.Services;
using Domain.Users.Repositories;
using MediatR;

namespace Domain.Addresses.Commands;

public record AddressUpdateCommand(int AddressId, AddressParams Params) : ICommand<Unit>;

internal class AddressUpdateCommandHandler(
    IAddressRepository addressRepository,
    IUserContextProvider userContext,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddressUpdateCommand, Unit>
{
    public async Task<Unit> Handle(AddressUpdateCommand command, CancellationToken cancellationToken)
    {
        var userEmail = userContext.Get().UserEmail;
        var user = await userRepository.FindByEmailAsync(userEmail, cancellationToken);
        
        if(!user.Addresses!.Any(a => a.Id == command.AddressId))
            throw new DomainException("Address not found", (int)AddressErrorCode.AddressNotFound);
        
        var address = await addressRepository.FindAsync(command.AddressId, cancellationToken);
        address.Update(command.Params);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}