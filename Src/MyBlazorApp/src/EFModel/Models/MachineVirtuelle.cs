public class MachineVirtuelle
{
    public int Id { get; set; }
    public string TypeOS { get; set; }
    public string TypeVM { get; set; }
    public string Sku { get; set; }
    public string Offer { get; set; }
    public string Version { get; set; }
    public string DiskISO { get; set; }
    public string NomMarketingVM { get; set; }
    public string FichierRDP { get; set; }
    public string Supervision { get; set; } // Traces d’utilisation

    public int StagiaireId { get; set; }
    public Utilisateur Stagiaire { get; set; }

    public int SalleDeFormationId { get; set; }
    public SalleDeFormation Salle { get; set; }
}