using Microsoft.EntityFrameworkCore;
using Warehouse.Models;

namespace Warehouse.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Item> Items {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("w_users");
        modelBuilder.Entity<Item>().ToTable("w_items");
    }
}