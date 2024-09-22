using Core.CQRS;
using Domain.Addresses.Dto;
using Domain.Addresses.Repositories;
using MediatR;

namespace Domain.Addresses.Queries;

public record AddressesGetQuery(int UserId) : IQuery<List<AddressDto>>;  

internal class AddressesGetQueryHandler(IAddressRepository addressRepository) : IRequestHandler<AddressesGetQuery, List<AddressDto>>
{
    public async Task<List<AddressDto>> Handle(AddressesGetQuery query, CancellationToken cancellationToken)
    {
        var addresses = await addressRepository.FindAllAsync(query.UserId, cancellationToken);
        var addressesDto = addresses.Select(address => new AddressDto(
            address.Street,
            address.Number,
            address.Apartment,
            address.City,
            address.PostalCode,
            address.Country,
            address.IsMain)).ToList();
        
        return addressesDto;
    }
}