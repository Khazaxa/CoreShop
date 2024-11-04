using Core.Database;
using Domain.Addresses.Entities;

namespace Domain.Addresses.Repositories;

internal interface IAddressRepository : IEntityRepository<Address>
{
}