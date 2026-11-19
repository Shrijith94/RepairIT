using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Dtos;

public class OrdinateurRequest
{
    [Required, StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Marque { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Processeur { get; set; } = string.Empty;

    [Range(1, 1024)]
    public int RamGo { get; set; }

    [Range(1, 100_000)]
    public int StockageGo { get; set; }

    [EnumDataType(typeof(TypeOrdinateur))]
    public TypeOrdinateur Type { get; set; }

    [EnumDataType(typeof(EtatOrdinateur))]
    public EtatOrdinateur Etat { get; set; }

    [Range(typeof(decimal), "0", "100000")]
    public decimal Prix { get; set; }

    [Range(0, 10_000)]
    public int QuantiteEnStock { get; set; }
}
