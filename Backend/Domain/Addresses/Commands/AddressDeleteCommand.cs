using Core.CQRS;
using Core.Exceptions;
using Domain.Addresses.Enum;
using Domain.Addresses.Repositories;
using MediatR;

namespace Domain.Addresses.Commands;

public record AddressDeleteCommand(int Id) : ICommand<Unit>;

internal class AddressDeleteCommandHandler(IAddressRepository addressRepository) : IRequestHandler<AddressDeleteCommand, Unit>
{
    public async Task<Unit> Handle(AddressDeleteCommand command, CancellationToken cancellationToken)
    {
        var address = await addressRepository.FindAsync(command.Id, cancellationToken) 
                      ?? throw new DomainException("Address not found", (int)AddressErrorCode.AddressNotFound);
        addressRepository.Delete(address);
        
        return Unit.Value;
    }
}