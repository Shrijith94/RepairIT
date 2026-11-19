using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

public interface IOrdinateurService
{
    Task<List<Ordinateur>> ObtenirOrdinateursAsync();
    Task<Ordinateur?> ObtenirOrdinateurAsync(int id);
    Task<Ordinateur> CreerOrdinateurAsync(OrdinateurRequest requete);
    Task<Ordinateur?> ModifierOrdinateurAsync(int id, OrdinateurRequest requete);
    Task<bool> SupprimerOrdinateurAsync(int id);
}
