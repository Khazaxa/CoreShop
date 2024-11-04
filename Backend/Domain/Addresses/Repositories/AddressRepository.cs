using Domain.Addresses.Entities;
using System.Linq;
using Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Domain.Addresses.Repositories;

internal class AddressRepository(IUnitOfWork unitOfWork) : EntityRepositoryBase<Address>(unitOfWork), IAddressRepository
{
    protected override IQueryable<Address> GetQuery()
    {
        return _dbSet;
    }
}