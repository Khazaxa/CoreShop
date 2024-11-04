using Domain.Addresses.Dto;
using Domain.Addresses.Entities;
using Domain.Users.Entities;

namespace Domain.Tests.Helpers;

internal class TestData
{
    public User User { get; set; }
    public Address Address { get; set; }
    public AddressDto AddressDto { get; set; }
}