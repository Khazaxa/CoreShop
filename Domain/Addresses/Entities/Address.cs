using System.ComponentModel.DataAnnotations;
using Core.Database;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Addresses.Entities;

internal class Address : EntityBase
{
    private Address () {}
    
    private const int MaxStreetLength = 100;
    private const int MaxCityLength = 60;
    
    
    public Address(
        string? street,
        string number,
        string? apartment,
        string city,
        string postalCode,
        string country,
        int userId)
    {
        Street = street;
        Number = number;
        Apartment = apartment;
        City = city;
        PostalCode = postalCode;
        Country = country;
        UserId = userId;
    }
    
    [MaxLength(MaxStreetLength)]
    public string? Street { get; init; }
    public string Number { get; init; }
    public string? Apartment { get; init; }
    [MaxLength(MaxCityLength)]
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }
    public string FullAddress => $"{Street} {Number}/{Apartment}, {PostalCode} {City}, {Country}";
    public int UserId { get; init; }
    public User User { get; init; }
    public bool IsMain { get; private set; }



    public void MakeMain() => IsMain = true;
    
    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Address>()
            .HasKey(a => a.Id);
        builder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId);
    }
}