using System;
using System.Collections.Generic;

public class SalleDeFormation
{
    public int Id { get; set; }
    public string NomFormation { get; set; }
    public int FormateurId { get; set; }
    public Utilisateur Formateur { get; set; }
    public ICollection<Utilisateur> Stagiaires { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    // Chaque salle référence une ou plusieurs machines virtuelles attribuées aux stagiaires
    public ICollection<MachineVirtuelle> Machines { get; set; }
}