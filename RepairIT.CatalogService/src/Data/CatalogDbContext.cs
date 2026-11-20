using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();

    protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=catalog.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ordinateur = modelBuilder.Entity<Ordinateur>();
        ordinateur.ToTable("Ordinateurs");
        ordinateur.HasKey(o => o.Id);
        ordinateur.Property(o => o.Nom).IsRequired().HasMaxLength(100);
        ordinateur.Property(o => o.Marque).IsRequired().HasMaxLength(50);
        ordinateur.Property(o => o.Processeur).IsRequired().HasMaxLength(100);
        ordinateur.Property(o => o.Prix).HasPrecision(10, 2);
        ordinateur.Property(o => o.Type).HasConversion<string>().HasMaxLength(20);
        ordinateur.Property(o => o.Etat).HasConversion<string>().HasMaxLength(20);
        ordinateur.Property(o => o.DateAjout)
            .HasConversion(date => date, date => DateTime.SpecifyKind(date, DateTimeKind.Utc));
        ordinateur.HasIndex(o => o.Nom);
    }
}
