using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepairIT.Shared.Configuration;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

public class CatalogDbInitializer(
    CatalogDbContext context,
    IOptions<DatabaseOptions> options,
    ILogger<CatalogDbInitializer> logger)
{
    public async Task InitialiserAsync()
    {
        if (options.Value.RecreerAuDemarrage)
        {
            await context.Database.EnsureDeletedAsync();
        }

        await context.Database.MigrateAsync();

        if (!options.Value.InsererDonneesInitiales || await context.Ordinateurs.AnyAsync())
        {
            return;
        }
        context.Ordinateurs.AddRange(
            new Ordinateur { Nom = "Dell XPS 13", Marque = "Dell", Processeur = "Intel Core i7", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Portable, Etat = EtatOrdinateur.Neuf, Prix = 1299m, QuantiteEnStock = 5, DateAjout = DateTime.UtcNow },
            new Ordinateur { Nom = "MacBook Air M2", Marque = "Apple", Processeur = "Apple M2", RamGo = 8, StockageGo = 256, Type = TypeOrdinateur.Portable, Etat = EtatOrdinateur.Reconditionne, Prix = 949m, QuantiteEnStock = 3, DateAjout = DateTime.UtcNow },
            new Ordinateur { Nom = "HP EliteDesk 800", Marque = "HP", Processeur = "Intel Core i5", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Fixe, Etat = EtatOrdinateur.Occasion, Prix = 489m, QuantiteEnStock = 8, DateAjout = DateTime.UtcNow },
            new Ordinateur { Nom = "Dell PowerEdge T150", Marque = "Dell", Processeur = "Intel Xeon E-2314", RamGo = 16, StockageGo = 2000, Type = TypeOrdinateur.Serveur, Etat = EtatOrdinateur.Neuf, Prix = 1590m, QuantiteEnStock = 1, DateAjout = DateTime.UtcNow });
        await context.SaveChangesAsync();
        logger.LogInformation("Base du catalogue initialisée avec des ordinateurs de démonstration");
    }
}
