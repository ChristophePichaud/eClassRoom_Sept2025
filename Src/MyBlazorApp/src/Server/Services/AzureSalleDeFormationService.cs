using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Services;
using Shared.Dtos;
using EFModel.Models;
using EFModel;

namespace Server.Services
{
    public class AzureSalleDeFormationService
    {
        private readonly AzureInfrastructureService _infra;
        private readonly EClassRoomDbContext _db;
        private readonly string _subscriptionId;
        private readonly string _location;

        public AzureSalleDeFormationService(AzureInfrastructureService infra, EClassRoomDbContext db, string subscriptionId, string location)
        {
            _infra = infra;
            _db = db;
            _subscriptionId = subscriptionId;
            _location = location;
        }

        /// <summary>
        /// Provisionne une salle de formation Azure avec une VM par stagiaire et stocke les IPs publiques.
        /// </summary>
        public async Task<ProvisionningResultDto> ProvisionnerSalleAsync(
            SalleDeFormationDto salle,
            string adminUser,
            string adminPassword,
            string tenantId,
            string clientId,
            string username,
            string password)
        {
            var result = new ProvisionningResultDto
            {
                Salle = salle
            };

            var armClient = AzureInfrastructureService.CreateArmClientWithUserPassword(
                _subscriptionId, tenantId, clientId, username, password);

            string rgName = $"rg-{salle.Nom}-{salle.Id}";
            await _infra.CreateResourceGroupAsync(rgName, _location);

            string vnetName = $"vnet-{salle.Nom}-{salle.Id}";
            string subnetName = $"subnet-{salle.Nom}-{salle.Id}";
            string addressPrefix = "10.0.0.0/16";
            string subnetPrefix = "10.0.0.0/24";
            var vnet = await _infra.CreateVirtualNetworkAsync(rgName, vnetName, _location, addressPrefix);
            var subnet = await _infra.CreateSubnetAsync(rgName, vnetName, subnetName, subnetPrefix);

            foreach (var stagiaire in salle.Stagiaires)
            {
                string vmName = $"vm-{salle.Nom}-{stagiaire.Nom}-{stagiaire.Id}";
                string ipName = $"ip-{vmName}";
                string nicName = $"nic-{vmName}";

                var publicIp = await _infra.CreatePublicIpAsync(rgName, ipName, _location);
                var nic = await _infra.CreateNetworkInterfaceAsync(
                    rgName,
                    nicName,
                    _location,
                    subnet.Id,
                    publicIp.Id);

                await _infra.CreateVirtualMachineAsync(
                    rgName,
                    vmName,
                    _location,
                    adminUser,
                    adminPassword,
                    nic.Id);

                // Ajout dans le résultat
                result.Stagiaires.Add(new ProvisionningResultDto.StagiaireVmInfo
                {
                    StagiaireId = stagiaire.Id,
                    StagiaireNom = stagiaire.Nom,
                    VmName = vmName,
                    PublicIp = publicIp.Data.IpAddress
                });

                // Stockage en base
                var provision = new ProvisionningVM
                {
                    SalleDeFormationId = salle.Id,
                    StagiaireId = stagiaire.Id,
                    VmName = vmName,
                    PublicIp = publicIp.Data.IpAddress,
                    DateProvisionning = DateTime.UtcNow
                };
                _db.ProvisionningVMs.Add(provision);
            }

            await _db.SaveChangesAsync();
            return result;
        }
    }
}
