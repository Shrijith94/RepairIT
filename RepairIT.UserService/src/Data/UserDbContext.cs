using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=users.db");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
