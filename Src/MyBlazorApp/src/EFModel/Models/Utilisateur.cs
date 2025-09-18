public enum RoleUtilisateur
{
    Administrateur,
    Formateur,
    Stagiaire
}

public class Utilisateur
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string MotDePasse { get; set; }
    public RoleUtilisateur Role { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
}