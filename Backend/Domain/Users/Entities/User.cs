using System.ComponentModel.DataAnnotations;
using Core.Database;
using Domain.Addresses.Entities;
using Domain.Users.Dto;
using Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Entities;

internal class User : EntityBase
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
    
    [MaxLength(NameMaxLength)]
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string AreaCode { get; private set; }
    public string Phone { get; private set; }
    [EmailAddress]
    [MaxLength(EmailMaxLength)]
    public string Email { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public List<int>? AddressId { get; private set; }
    public List<Address>? Addresses { get; private set; }
    public UserRole Role { get; private set; }
    
    public void Update(UserParams userParams)
    {
        Name = userParams.Name;
        Surname = userParams.Surname;
        AreaCode = userParams.AreaCode;
        Phone = userParams.Phone;
        Email = userParams.Email;
        Role = userParams.Role;
    }
    
    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<User>()
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
    }
}