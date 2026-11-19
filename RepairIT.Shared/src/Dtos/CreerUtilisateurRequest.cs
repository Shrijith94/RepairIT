using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Dtos;

public class CreerUtilisateurRequest
{
    [Required, StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Prenom { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [EnumDataType(typeof(RoleUtilisateur))]
    public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Gestionnaire;
}
