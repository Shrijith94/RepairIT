using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Models;

public class Utilisateur
{
    public int Id { get; set; }
    public string UserIdKeycloak { get; set; } = "non-associe";
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RoleUtilisateur Role { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; }
}
