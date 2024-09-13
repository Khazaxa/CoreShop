using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Addresses.Entities;

internal class Address
{
    private Address () {}
    
    private const int MaxStreetLength = 100;
    private const int MaxCityLength = 60;
    
    
    public Address(
        string? street,
        int number,
        int? apartment,
        string city,
        string postalCode,
        string country)
    {
        Street = street;
        Number = number;
        Apartment = apartment;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }
    
    public int Id { get; init; }
    [MaxLength(MaxStreetLength)]
    public string? Street { get; init; }
    public int Number { get; init; }
    public int? Apartment { get; init; }
    [MaxLength(MaxCityLength)]
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }
    public string FullAddress => $"{Street} {Number}/{Apartment}, {PostalCode} {City}, {Country}";
    public int UserId { get; init; }
    
    
    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Address>()
            .HasKey(a => a.Id);
    }
}