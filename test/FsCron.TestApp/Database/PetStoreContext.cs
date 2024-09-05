using FsCron.TestApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsCron.TestApp.Database;

public class PetStoreContext : DbContext
{
    private readonly string _dbPath;

    public PetStoreContext()
    {
        var path = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
        );
        path = "/Users/michalmoudry/";
        _dbPath = Path.Join(path, "petstore.db");
    }

    public DbSet<Pet> Pets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={_dbPath}");
}