using System;
using System.Collections.Generic;

namespace EFModel.Models
{
    public class ProvisionningVM
    {
        public int Id { get; set; }
        public int SalleDeFormationId { get; set; }
        public SalleDeFormation SalleDeFormation { get; set; }
        public int StagiaireId { get; set; }
        public Utilisateur Stagiaire { get; set; }
        public string VmName { get; set; }
        public string PublicIp { get; set; }
        public DateTime DateProvisionning { get; set; }
    }
}
