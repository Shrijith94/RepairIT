using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Ordinateur>>> ObtenirOrdinateurs()
    {
        logger.LogInformation("GET - api/ordinateurs/ - Récupération de tous les ordinateurs");
        return StatusCode(200, await service.ObtenirOrdinateursAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Ordinateur>> ObtenirOrdinateur(int id)
    {
        var ordinateur = await service.ObtenirOrdinateurAsync(id);
        if(ordinateur is null)
        {
            logger.LogWarning("GET - api.ordinateurs/"+ id +" - Ordinateur non trouvé");
            return StatusCode(404, new { message = "Ordinateur introuvable" });
        }
        logger.LogInformation("GET - api/ordinateurs/"+ id +" - Récupération de l'ordinateur avec l'id " + id);
        return StatusCode(200, new { ordinateur });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Ordinateur>> CreerOrdinateur(OrdinateurRequest requete)
    {
        await service.CreerOrdinateurAsync(requete);
        logger.LogInformation("POST - api/ordinateurs/ - Ajout d'un ordinateur");
        return StatusCode(201, new { message = "Ordinateur ajouté avec succès" }); 
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Ordinateur>> ModifierOrdinateur(int id, OrdinateurRequest requete)
    {
        var ordinateur = await service.ModifierOrdinateurAsync(id, requete);
        if(ordinateur is null)
        {
            logger.LogWarning("PUT - api/ordinateurs/"+ id +" - Ordinateur non trouvé");
            return StatusCode(404, new { message = "Ordinateur introuvable" });
        }
        logger.LogInformation("PUT - api/ordinateurs/"+ id +" - Modification de l'ordinateur " + id);
        return StatusCode(200, new { message = "Ordinateur modifié avec succès", ordinateur });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SupprimerOrdinateur(int id)
    {
        var ordinateurSupprime = await service.SupprimerOrdinateurAsync(id);
        if(!ordinateurSupprime)
        {
            logger.LogWarning("DELETE - api/ordinateurs/"+ id +" - Ordinateur non trouvé");
            return StatusCode(404, new { message = "Ordinateur introuvable" });
        }
        logger.LogInformation("DELETE - api/ordinateurs/"+ id +" - Suppression de l'ordinateur " + id);
        return StatusCode(204, new { message = "Ordinateur supprimé avec succès" });
    }
        
}
