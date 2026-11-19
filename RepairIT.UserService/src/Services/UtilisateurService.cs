using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;
using RepairIT.Data;

namespace RepairIT.UserService.Services;

public class UtilisateurService(RepairItDbContext context) : IUtilisateurService
{
    public Task<List<Utilisateur>> ObtenirUtilisateursAsync()
    {
        return context.Utilisateurs.AsNoTracking().OrderBy(u => u.Nom).ToListAsync();
    }

    public Task<Utilisateur?> ObtenirUtilisateurAsync(int id) =>
        context.Utilisateurs.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Utilisateur> CreerUtilisateurAsync(CreerUtilisateurRequest requete)
    {
        var utilisateur = new Utilisateur
        {
            Nom = requete.Nom,
            Prenom = requete.Prenom,
            Email = requete.Email,
            Role = requete.Role,
            EstActif = true,
            DateCreation = DateTime.UtcNow
        };

        context.Utilisateurs.Add(utilisateur);
        await context.SaveChangesAsync();
        return utilisateur;
    }
}
