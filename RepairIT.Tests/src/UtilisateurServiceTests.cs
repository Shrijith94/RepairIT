using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.Data;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Enums;
using RepairIT.UserService.Services;

namespace RepairIT.Tests;

public class UtilisateurServiceTests
{
    private readonly RepairItDbContext _context;
    private readonly UtilisateurService _service;

    public UtilisateurServiceTests()
    {
        _context = CreerContexte();
        _service = new UtilisateurService(_context);
    }

    private static RepairItDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<RepairItDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new RepairItDbContext(options);
    }

    private static CreerUtilisateurRequest CreerRequeteUtilisateur(string nom = "Durand") => new()
    {
        Nom = nom,
        Prenom = "Paul",
        Email = $"paul.{nom.ToLower()}@repairit.fr",
        Role = RoleUtilisateur.Gestionnaire
    };

    [Fact]
    public async Task CreerUtilisateurActifAvecLaValeurKeycloakParDefaut()
    {
        var requete = CreerRequeteUtilisateur();

        var utilisateur = await _service.CreerUtilisateurAsync(requete);

        utilisateur.Id.Should().NotBe(0);
        utilisateur.Should().BeEquivalentTo(requete);
        utilisateur.UserIdKeycloak.Should().Be("non-associe");
        utilisateur.EstActif.Should().BeTrue();
        utilisateur.DateCreation.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task CreerUtilisateurConserveLeRoleDemande()
    {
        var requete = CreerRequeteUtilisateur();
        requete.Role = RoleUtilisateur.Administrateur;

        var utilisateur = await _service.CreerUtilisateurAsync(requete);
        var resultat = await _service.ObtenirUtilisateurAsync(utilisateur.Id);

        resultat!.Role.Should().Be(RoleUtilisateur.Administrateur);
    }

    [Fact]
    public async Task ObtenirUtilisateur()
    {
        var utilisateur = await _service.CreerUtilisateurAsync(CreerRequeteUtilisateur());

        var resultat = await _service.ObtenirUtilisateurAsync(utilisateur.Id);

        resultat.Should().BeEquivalentTo(utilisateur);
    }

    [Fact]
    public async Task ObtenirUtilisateurNullSiLUtilisateurNExistePas()
    {
        var resultat = await _service.ObtenirUtilisateurAsync(42);

        resultat.Should().BeNull();
    }

    [Fact]
    public async Task ObtenirUtilisateursTriesParNom()
    {
        await _service.CreerUtilisateurAsync(CreerRequeteUtilisateur("Martin"));
        await _service.CreerUtilisateurAsync(CreerRequeteUtilisateur("Bernard"));
        await _service.CreerUtilisateurAsync(CreerRequeteUtilisateur("Durand"));

        var resultat = await _service.ObtenirUtilisateursAsync();

        resultat.Select(u => u.Nom).Should().Equal("Bernard", "Durand", "Martin");
    }
}
