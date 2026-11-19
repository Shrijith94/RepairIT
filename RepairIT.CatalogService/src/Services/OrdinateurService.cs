using Microsoft.EntityFrameworkCore;
using RepairIT.Data;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

public class OrdinateurService(RepairItDbContext context) : IOrdinateurService
{
    public Task<List<Ordinateur>> ObtenirOrdinateursAsync(){
        return context.Ordinateurs.Select(o => o).OrderBy(o => o.Nom).ToListAsync();
    }

    public Task<Ordinateur?> ObtenirOrdinateurAsync(int id) =>
        context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Ordinateur> CreerOrdinateurAsync(OrdinateurRequest requete)
    {
        var ordinateur = new Ordinateur { DateAjout = DateTime.UtcNow };
        AppliquerRequete(ordinateur, requete);

        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync();
        return ordinateur;
    }

    public async Task<Ordinateur?> ModifierOrdinateurAsync(int id, OrdinateurRequest requete)
    {
        var ordinateur = await ObtenirOrdinateurAsync(id);
        if (ordinateur is null)
        {
            return null;
        }
        AppliquerRequete(ordinateur, requete);
        await context.SaveChangesAsync();
        return ordinateur;
    }

    public async Task<bool> SupprimerOrdinateurAsync(int id)
    {
        var ordinateur = await ObtenirOrdinateurAsync(id);
        if (ordinateur != null)
        {
            context.Ordinateurs.Remove(ordinateur);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    private static void AppliquerRequete(Ordinateur ordinateur, OrdinateurRequest requete)
    {
        ordinateur.Nom = requete.Nom;
        ordinateur.Marque = requete.Marque;
        ordinateur.Processeur = requete.Processeur;
        ordinateur.RamGo = requete.RamGo;
        ordinateur.StockageGo = requete.StockageGo;
        ordinateur.Type = requete.Type;
        ordinateur.Etat = requete.Etat;
        ordinateur.Prix = requete.Prix;
        ordinateur.QuantiteEnStock = requete.QuantiteEnStock;
    }
}
