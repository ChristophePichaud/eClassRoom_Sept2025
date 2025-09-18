using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AzureInfrastructureService
    {
        private readonly ArmClient _armClient;
        private readonly string _subscriptionId;

        public AzureInfrastructureService(string subscriptionId)
        {
            _subscriptionId = subscriptionId;
            _armClient = new ArmClient(new DefaultAzureCredential(), subscriptionId);
        }

        public async Task<ResourceGroupResource> CreateResourceGroupAsync(string rgName, string location)
        {
            var rgCollection = _armClient.GetDefaultSubscription().GetResourceGroups();
            var rgData = new ResourceGroupData(location);
            var rgLro = await rgCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, rgName, rgData);
            return rgLro.Value;
        }

        public async Task<VirtualMachineResource> CreateVirtualMachineAsync(
            string rgName,
            string vmName,
            string location,
            string adminUser,
            string adminPassword,
            string nicResourceId)
        {
            var resourceGroup = await _armClient.GetDefaultSubscription().GetResourceGroups().GetAsync(rgName);
            var vmCollection = resourceGroup.Value.GetVirtualMachines();

            var vmData = new VirtualMachineData(location)
            {
                HardwareProfile = new HardwareProfile { VmSize = VirtualMachineSizeType.StandardD2V3 },
                StorageProfile = new StorageProfile
                {
                    OSDisk = new OSDisk(DiskCreateOptionType.FromImage)
                    {
                        Name = $"{vmName}-osdisk",
                        Caching = CachingType.ReadWrite,
                        ManagedDisk = new ManagedDiskParameters { StorageAccountType = StorageAccountType.StandardLrs }
                    },
                    ImageReference = new ImageReference
                    {
                        Publisher = "MicrosoftWindowsDesktop",
                        Offer = "Windows-10",
                        Sku = "win10-21h2-pro",
                        Version = "latest"
                    }
                },
                OSProfile = new OSProfile
                {
                    ComputerName = vmName,
                    AdminUsername = adminUser,
                    AdminPassword = adminPassword
                },
                NetworkProfile = new NetworkProfile
                {
                    NetworkInterfaces = {
                        new NetworkInterfaceReference { Id = nicResourceId, Primary = true }
                    }
                }
            };

            var vmLro = await vmCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, vmName, vmData);
            return vmLro.Value;
        }

        public async Task<VirtualNetworkResource> CreateVirtualNetworkAsync(string rgName, string vnetName, string location, string addressPrefix)
        {
            var resourceGroup = await _armClient.GetDefaultSubscription().GetResourceGroups().GetAsync(rgName);
            var vnetCollection = resourceGroup.Value.GetVirtualNetworks();
            var vnetData = new VirtualNetworkData()
            {
                Location = location,
                AddressPrefixes = { addressPrefix }
            };
            var vnetLro = await vnetCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, vnetName, vnetData);
            return vnetLro.Value;
        }

        public async Task<SubnetResource> CreateSubnetAsync(string rgName, string vnetName, string subnetName, string addressPrefix)
        {
            var resourceGroup = await _armClient.GetDefaultSubscription().GetResourceGroups().GetAsync(rgName);
            var vnet = await resourceGroup.Value.GetVirtualNetworks().GetAsync(vnetName);
            var subnetCollection = vnet.Value.GetSubnets();
            var subnetData = new SubnetData() { AddressPrefix = addressPrefix };
            var subnetLro = await subnetCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, subnetName, subnetData);
            return subnetLro.Value;
        }

        public async Task<PublicIPAddressResource> CreatePublicIpAsync(string rgName, string ipName, string location)
        {
            var resourceGroup = await _armClient.GetDefaultSubscription().GetResourceGroups().GetAsync(rgName);
            var ipCollection = resourceGroup.Value.GetPublicIPAddresses();
            var ipData = new PublicIPAddressData()
            {
                Location = location,
                PublicIPAllocationMethod = IPAllocationMethod.Static,
                Sku = new PublicIPAddressSku() { Name = PublicIPAddressSkuName.Standard }
            };
            var ipLro = await ipCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, ipName, ipData);
            return ipLro.Value;
        }

        public async Task<NetworkInterfaceResource> CreateNetworkInterfaceAsync(
            string rgName,
            string nicName,
            string location,
            string subnetId,
            string publicIpId)
        {
            var resourceGroup = await _armClient.GetDefaultSubscription().GetResourceGroups().GetAsync(rgName);
            var nicCollection = resourceGroup.Value.GetNetworkInterfaces();
            var nicData = new NetworkInterfaceData()
            {
                Location = location,
                IpConfigurations =
                {
                    new NetworkInterfaceIPConfigurationData()
                    {
                        Name = $"{nicName}-ipconfig",
                        Subnet = new SubnetData() { Id = subnetId },
                        PublicIPAddress = new PublicIPAddressData() { Id = publicIpId }
                    }
                }
            };
            var nicLro = await nicCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, nicName, nicData);
            return nicLro.Value;
        }

        /// <summary>
        /// Connexion à Azure avec un compte technique (username/password).
        /// </summary>
        public static ArmClient CreateArmClientWithUserPassword(string subscriptionId, string tenantId, string clientId, string username, string password)
        {
            var credential = new UsernamePasswordCredential(
                username: username,
                password: password,
                tenantId: tenantId,
                clientId: clientId
            );
            return new ArmClient(credential, subscriptionId);
        }

        // Ajoutez ici d'autres méthodes si nécessaire
    }
}
