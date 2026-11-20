using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Models;

public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Marque { get; set; } = string.Empty;
    public string Processeur { get; set; } = string.Empty;
    public int RamGo { get; set; }
    public int StockageGo { get; set; }
    public TypeOrdinateur Type { get; set; }
    public EtatOrdinateur Etat { get; set; }
    public decimal Prix { get; set; }
    public int QuantiteEnStock { get; set; }
    public DateTime DateAjout { get; set; }
}
