namespace Shared.Dtos
{
    public class UtilisateurDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string MotDePasse { get; set; }
        public string Role { get; set; }
        public int? ClientId { get; set; } // Ajout pour lier à une société
    }
}
