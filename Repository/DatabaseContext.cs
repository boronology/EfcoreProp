
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfcoreProp.Repository;

class DatabaseContext : DbContext
{
    public DbSet<DbBook> Books { get; set; }

    public DbSet<DbPublisher> Publishers { get; set; }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<Guid>().HaveConversion<GuidToStringConverter>();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DbBook>()
            .HasOne(e => e.Publisher)
            .WithMany(e => e.Books)
            .HasForeignKey(e => e.PublisherId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var builder = new SqliteConnectionStringBuilder()
        {
            ForeignKeys = true,
            DataSource = "database.db",
        };
        optionsBuilder.UseSqlite(builder.ToString());
    }
}