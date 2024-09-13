using Domain.Addresses.Entities;

namespace Domain.Addresses.Repositories;

internal interface IAddressRepository
{
    Task<Address> AddAsync(Address address, CancellationToken cancellationToken);
    Task<Address?> FindAsync(int addressId, CancellationToken cancellationToken);
    Task<List<Address>> FindAllAsync(int userId, CancellationToken cancellationToken);
}