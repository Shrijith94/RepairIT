namespace RepairIT.Shared.Configuration;

public class DatabaseOptions
{
    public const string Section = "Database";

    public bool RecreerAuDemarrage { get; set; }

    public bool InsererDonneesInitiales { get; set; } = true;
}
