using Microsoft.EntityFrameworkCore;
using PvPet.Data.Entities;

namespace PvPet.Data.Contexts;

public class PvPetDbContext : DbContext
{
    public PvPetDbContext(DbContextOptions<PvPetDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Pet> Pets { get; set; }

    public DbSet<ItemOnMap> ItemsOnMap { get; set; }

    public DbSet<Item> Items { get; set; }

    public async Task InitAsync()
    {
        await Database.EnsureCreatedAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        AddConstraints(modelBuilder);
        AddForeignKeys(modelBuilder);
        SeedData(modelBuilder);

    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        
    }

    private void SeedUsers(ModelBuilder modelBuilder)
    {
        
    }

    private void AddForeignKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(acc => acc.Pet)
            .WithOne(pet => pet.User);

        modelBuilder.Entity<Pet>()
            .HasMany(pet => pet.Items)
            .WithOne(item => item.Pet)
            .HasForeignKey(item => item.PetId);

        modelBuilder.Entity<Pet>()
            .Property(e => e.Location)
            .HasColumnType("geography (point)");

        modelBuilder.Entity<ItemOnMap>()
            .Property(e => e.Location)
            .HasColumnType("geography (point)");
    }

    private void AddConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();
    }
}
