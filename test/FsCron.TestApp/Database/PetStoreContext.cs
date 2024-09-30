using FsCron.TestApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsCron.TestApp.Database;

public sealed class PetStoreContext : DbContext
{
    private const string DbPath = "petstore.db";

    public DbSet<Pet> Pets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}