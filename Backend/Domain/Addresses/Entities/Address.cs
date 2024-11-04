using System.ComponentModel.DataAnnotations;
using Core.Database;
using Domain.Addresses.Dto;
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
    public string? Street { get; private set; }
    public string Number { get; private set; }
    public string? Apartment { get; private set; }
    [MaxLength(MaxCityLength)]
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    public string FullAddress => $"{Street} {Number}/{Apartment}, {PostalCode} {City}, {Country}";
    public int UserId { get; private set; }
    public User User { get; private set; }
    public bool IsMain { get; private set; }

    
    public void MakeMain() => IsMain = true;
    
    public void Update(AddressParams addressParams)
    {
        Street = addressParams.Street;
        Number = addressParams.Number;
        Apartment = addressParams.Apartment;
        City = addressParams.City;
        PostalCode = addressParams.PostalCode;
        Country = addressParams.Country;
    }
    
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