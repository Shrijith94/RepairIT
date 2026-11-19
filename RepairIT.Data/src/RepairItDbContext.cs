using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.Data;

public class RepairItDbContext(DbContextOptions<RepairItDbContext> options) : DbContext(options)
{
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

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

        var utilisateur = modelBuilder.Entity<Utilisateur>();
        utilisateur.ToTable("Utilisateurs");
        utilisateur.HasKey(u => u.Id);
        utilisateur.Property(u => u.UserIdKeycloak).IsRequired().HasMaxLength(100);
        utilisateur.Property(u => u.Nom).IsRequired().HasMaxLength(100);
        utilisateur.Property(u => u.Prenom).IsRequired().HasMaxLength(100);
        utilisateur.Property(u => u.Email).IsRequired().HasMaxLength(200);
        utilisateur.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        utilisateur.Property(u => u.DateCreation)
            .HasConversion(date => date, date => DateTime.SpecifyKind(date, DateTimeKind.Utc));
    }
}
