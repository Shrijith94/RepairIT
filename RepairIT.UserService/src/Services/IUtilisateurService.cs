using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Services;

public interface IUtilisateurService
{
    Task<List<Utilisateur>> ObtenirUtilisateursAsync();
    Task<Utilisateur?> ObtenirUtilisateurAsync(int id);
    Task<Utilisateur> CreerUtilisateurAsync(CreerUtilisateurRequest requete);
}
