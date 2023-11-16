using Microsoft.EntityFrameworkCore;
using PvPet.Data.Entities;

namespace PvPet.Data.Contexts;

public class PvPetDbContext : DbContext
{
    public PvPetDbContext(DbContextOptions<PvPetDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }

    public DbSet<Account> Users { get; set; }

    public DbSet<Pet> Pets { get; set; }

    public async Task InitAsync()
    {
        await Database.EnsureCreatedAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);

        modelBuilder.Entity<Account>()
            .HasOne(acc => acc.Pet)
            .WithOne(pet => pet.Account);

        modelBuilder.Entity<Pet>()
            .HasMany(pet => pet.Items)
            .WithOne()
            .HasForeignKey(item => item.PetId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void AddConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasIndex(account => account.Username)
            .IsUnique();
    }
}
