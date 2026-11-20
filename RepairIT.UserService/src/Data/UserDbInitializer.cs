using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepairIT.Shared.Configuration;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Data;

public class UserDbInitializer(
    UserDbContext context,
    IOptions<DatabaseOptions> options,
    ILogger<UserDbInitializer> logger)
{
    public async Task InitialiserAsync()
    {
        if (options.Value.RecreerAuDemarrage)
        {
            await context.Database.EnsureDeletedAsync();
        }

        await context.Database.MigrateAsync();

        if (!options.Value.InsererDonneesInitiales || await context.Utilisateurs.AnyAsync())
        {
            return;
        }
        context.Utilisateurs.AddRange(
            new Utilisateur { Nom = "Martin", Prenom = "Alice", Email = "alice.martin@repairit.fr", Role = RoleUtilisateur.Administrateur, DateCreation =  DateTime.UtcNow },
            new Utilisateur { Nom = "Bernard", Prenom = "Lucas", Email = "lucas.bernard@repairit.fr", Role = RoleUtilisateur.Gestionnaire, DateCreation =  DateTime.UtcNow });
        await context.SaveChangesAsync();
        logger.LogInformation("Base des utilisateurs initialisée avec des comptes de démonstration");
    }
}
