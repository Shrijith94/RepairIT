using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilisateursController(IUtilisateurService service, ILogger<UtilisateursController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Utilisateur>>> ObtenirUtilisateurs()
    {
        logger.LogInformation("GET - api/utilisateurs/ - Récupération de tous les utilisateurs");
        return StatusCode(200, await service.ObtenirUtilisateursAsync());
    } 

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Utilisateur>> ObtenirUtilisateur(int id)
    {
        var utilisateur = await service.ObtenirUtilisateurAsync(id);
        if(utilisateur is null)
        {
            logger.LogWarning("GET - api/utilisateurs/"+ id +" - Utilisateur non trouvé");
            return StatusCode(404, new { message = "Utilisateur introuvable" });
        }
        logger.LogInformation("GET - api/utilisateurs/"+ id +" - Récupération de l'utilisateur " + id);
        return StatusCode(200, new { utilisateur });
    }

    [HttpPost]
    public async Task<ActionResult<Utilisateur>> CreerUtilisateur(CreerUtilisateurRequest requete)
    {
        var utilisateur = await service.CreerUtilisateurAsync(requete);
        logger.LogInformation("POST - api/utilisateurs/ - Création d'un nouvel utilisateur");
        return StatusCode(201, new { message = "Utilisateur créé avec succès", utilisateur });
    }
}
