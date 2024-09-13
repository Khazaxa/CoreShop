using Domain.Addresses.Entities;
using System.Linq;

namespace Domain.Addresses.Repositories;

internal class AddressRepository(ShopDbContext dbContext) : IAddressRepository
{
    public async Task<Address> AddAsync(Address address, CancellationToken cancellationToken)
    {
        await dbContext.Addresses.AddAsync(address, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return address;
    }

    public async Task<Address?> FindAsync(int addressId, CancellationToken cancellationToken)
    {
        var address = dbContext.Addresses.FirstOrDefault(a => a.Id == addressId);
        return await Task.FromResult(address);
    }
}