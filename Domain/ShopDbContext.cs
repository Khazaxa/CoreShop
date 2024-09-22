using Domain.Addresses.Entities;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

internal class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        User.OnModelCreating(modelBuilder);
        Address.OnModelCreating(modelBuilder);
    }
}