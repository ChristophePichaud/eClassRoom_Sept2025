using System.Collections.Generic;

namespace EFModel.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string NomSociete { get; set; }
        public string Adresse { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Pays { get; set; }
        public string EmailAdministrateur { get; set; }
        public string Mobile { get; set; }
        public string MotDePasseAdministrateur { get; set; }

        public ICollection<Utilisateur> Utilisateurs { get; set; }
        public ICollection<SalleDeFormation> Salles { get; set; }
    }
}