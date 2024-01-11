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

    public async Task InitAsync()
    {
        await Database.EnsureCreatedAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        AddConstraints(modelBuilder);
        AddForeignKeys(modelBuilder);
        AddIndexes(modelBuilder);
    }

    private static void AddForeignKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Pet)
            .WithOne(pet => pet.User);

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.ShopItems)
            .WithOne(i => i.Pet);

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.Items)
            .WithOne(i => i.Pet);

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.Fights)
            .WithMany(f => f.Pets);

        modelBuilder.Entity<Fight>()
            .HasMany(f => f.Pets)
            .WithMany(p => p.Fights);

        modelBuilder.Entity<Fight>()
            .HasMany(f => f.Rounds)
            .WithOne(r  => r.Fight);
    }

    private static void AddConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();

        modelBuilder.Entity<Pet>()
            .Property(e => e.Location)
            .HasColumnType("geography (point)");

        modelBuilder.Entity<ItemOnMap>()
            .Property(e => e.Location)
            .HasColumnType("geography (point)");
    }

    private static void AddIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>()
            .HasIndex(p => p.Location);
    }
}
