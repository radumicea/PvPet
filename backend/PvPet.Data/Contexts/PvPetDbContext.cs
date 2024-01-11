﻿using Microsoft.EntityFrameworkCore;
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
            .HasOne(u => u.Pet)
            .WithOne(pet => pet.User);

        modelBuilder.Entity<User>()
            .HasMany(u => u.ShopItems)
            .WithOne(item => item.User);

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
