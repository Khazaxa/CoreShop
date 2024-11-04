using Core.CQRS;
using Domain.Addresses.Dto;
using Domain.Addresses.Repositories;
using MediatR;

namespace Domain.Addresses.Queries;

public record AddressDetailsQuery(int AddressId) : IQuery<AddressDto>;  

internal class AddressesGetQueryHandler(
    IAddressRepository addressRepository
) : IRequestHandler<AddressDetailsQuery, AddressDto>
{
    public async Task<AddressDto> Handle(AddressDetailsQuery query, CancellationToken cancellationToken)
    {
        var address = await addressRepository.FindAsync(query.AddressId, cancellationToken);
        var addressDto = new AddressDto(
            address.Street,
            address.Number,
            address.Apartment,
            address.City,
            address.PostalCode,
            address.Country,
            address.IsMain
        );
        
        return addressDto;
    }
}