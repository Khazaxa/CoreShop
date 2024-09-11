using System.ComponentModel.DataAnnotations;
using Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Entities;

internal class User
{
    public const int EmailMaxLength = 64;
    public const int NameMaxLength = 64;

    private User() {}

    public User(string userName, string email, byte[] passwordHash, byte[] passwordSalt, UserRole role)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Role = role;
    }
    
    
    public int Id { get; private init; }
    [MaxLength(NameMaxLength)]
    public string UserName { get; private init; }
    [MaxLength(EmailMaxLength)]
    public string Email { get; private init; }
    public byte[] PasswordHash { get; private init; }
    public byte[] PasswordSalt { get; private init; }
    public UserRole Role { get; private init; }
    

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        // builder.Entity<User>()
        //     .HasOne(u => u.Desk)
        //     .WithOne(d => d.User)
        //     .HasForeignKey<User>(u => u.DeskId)
        //     .OnDelete(DeleteBehavior.SetNull);
    }
}