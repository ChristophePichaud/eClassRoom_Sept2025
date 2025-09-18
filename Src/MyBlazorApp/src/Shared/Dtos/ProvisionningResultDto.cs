using System.Collections.Generic;

namespace Shared.Dtos
{
    public class ProvisionningResultDto
    {
        public SalleDeFormationDto Salle { get; set; }
        public List<StagiaireVmInfo> Stagiaires { get; set; } = new();

        public class StagiaireVmInfo
        {
            public int StagiaireId { get; set; }
            public string StagiaireNom { get; set; }
            public string VmName { get; set; }
            public string PublicIp { get; set; }
        }
    }
}
