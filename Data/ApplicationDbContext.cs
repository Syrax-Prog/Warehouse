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
    public DbSet<Supplier> Suppliers {get; set;}
    public DbSet<Inbound> Inbounds {get; set;}
    public DbSet<InboundItem> InboundItems {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("w_users");
        modelBuilder.Entity<Item>().ToTable("w_items");
        modelBuilder.Entity<Inbound>().ToTable("w_inbounds");
        modelBuilder.Entity<InboundItem>().ToTable("w_inbound_items");
        modelBuilder.Entity<Supplier>().ToTable("w_supplier");
    }
}