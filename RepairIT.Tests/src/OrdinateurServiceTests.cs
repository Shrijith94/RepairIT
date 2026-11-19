using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.CatalogService.Services;
using RepairIT.Data;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Enums;

namespace RepairIT.Tests;

public class OrdinateurServiceTests
{
    private readonly RepairItDbContext _context;
    private readonly OrdinateurService _service;

    public OrdinateurServiceTests()
    {
        _context = CreerContexte();
        _service = new OrdinateurService(_context);
    }

    private static RepairItDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<RepairItDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new RepairItDbContext(options);
    }

    private static OrdinateurRequest CreerRequeteOrdinateur(string nom = "Dell XPS 13") => new()
    {
        Nom = nom,
        Marque = "Dell",
        Processeur = "Intel Core i7",
        RamGo = 16,
        StockageGo = 512,
        Type = TypeOrdinateur.Portable,
        Etat = EtatOrdinateur.Neuf,
        Prix = 1299m,
        QuantiteEnStock = 5
    };

    [Fact]
    public async Task CreerOrdinateur()
    {
        var requete = CreerRequeteOrdinateur();

        var ordinateur = await _service.CreerOrdinateurAsync(requete);
        var resultat = await _service.ObtenirOrdinateurAsync(ordinateur.Id);

        ordinateur.Id.Should().NotBe(0);
        resultat.Should().NotBeNull();
        resultat.Should().BeEquivalentTo(requete);
        resultat!.DateAjout.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task ObtenirOrdinateursTriesParNom()
    {
        await _service.CreerOrdinateurAsync(CreerRequeteOrdinateur("MacBook Air"));
        await _service.CreerOrdinateurAsync(CreerRequeteOrdinateur("Dell XPS 13"));
        await _service.CreerOrdinateurAsync(CreerRequeteOrdinateur("HP EliteDesk"));

        var resultat = await _service.ObtenirOrdinateursAsync();

        resultat.Select(o => o.Nom).Should().Equal("Dell XPS 13", "HP EliteDesk", "MacBook Air");
    }

    [Fact]
    public async Task ObtenirOrdinateurNullSiLOrdinateurNExistePas()
    {
        var resultat = await _service.ObtenirOrdinateurAsync(42);

        resultat.Should().BeNull();
    }

    [Fact]
    public async Task ModifierOrdinateurMetAJourLesChamps()
    {
        var ordinateur = await _service.CreerOrdinateurAsync(CreerRequeteOrdinateur());
        var modification = CreerRequeteOrdinateur("Dell XPS 15");
        modification.Prix = 1599m;
        modification.Etat = EtatOrdinateur.Reconditionne;

        await _service.ModifierOrdinateurAsync(ordinateur.Id, modification);
        var resultat = await _service.ObtenirOrdinateurAsync(ordinateur.Id);

        resultat.Should().BeEquivalentTo(modification);
    }

    [Fact]
    public async Task ModifierOrdinateurRetourneNullSiLOrdinateurNExistePas()
    {
        var resultat = await _service.ModifierOrdinateurAsync(42, CreerRequeteOrdinateur());

        resultat.Should().BeNull();
    }

    [Fact]
    public async Task SupprimerOrdinateurRetourneTrue()
    {
        var ordinateur = await _service.CreerOrdinateurAsync(CreerRequeteOrdinateur());

        var resultatSuppression = await _service.SupprimerOrdinateurAsync(ordinateur.Id);
        var resultat = await _service.ObtenirOrdinateurAsync(ordinateur.Id);

        resultatSuppression.Should().BeTrue();
        resultat.Should().BeNull();
    }

    [Fact]
    public async Task SupprimerOrdinateurRetourneFalseSiLOrdinateurNExistePas()
    {
        var resultatSuppression = await _service.SupprimerOrdinateurAsync(42);
        resultatSuppression.Should().BeFalse();

        var resultat = await _service.ObtenirOrdinateurAsync(42);
        resultat.Should().BeNull();
    }
}
