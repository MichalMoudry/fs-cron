using FsCron.TestApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsCron.TestApp.Database;

public sealed class TestDbContext : DbContext
{
    private const string DbPath = "test_db.db";

    public DbSet<JobResult> JobResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}