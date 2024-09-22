using System.ComponentModel.DataAnnotations;
using Domain.Addresses.Entities;
using Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Entities;

internal class User
{
    public const int EmailMaxLength = 64;
    public const int NameMaxLength = 64;

    private User() {}

    public User(
        string name, 
        string surname, 
        string areaCode,
        string phone, 
        string email, 
        byte[] passwordHash,
        byte[] passwordSalt, 
        UserRole role)
    {
        Name = name;
        Surname = surname;
        AreaCode = areaCode;
        Phone = phone;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Role = role;
    }
    
    public int Id { get; private init; }
    [MaxLength(NameMaxLength)]
    public string Name { get; private init; }
    public string Surname { get; private init; }
    public string AreaCode { get; private init; }
    public string Phone { get; private init; }
    [EmailAddress]
    [MaxLength(EmailMaxLength)]
    public string Email { get; private init; }
    public byte[] PasswordHash { get; private init; }
    public byte[] PasswordSalt { get; private init; }
    public List<int>? AddressId { get; private init; }
    public List<Address>? Addresses { get; private init; }
    public UserRole Role { get; private init; }
    
    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<User>()
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
    }
}