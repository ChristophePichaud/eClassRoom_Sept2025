# Infrastructure Azure : création modulaire avec Azure SDK .NET 2.x

## Vue d’ensemble

Ce service permet de créer des ressources Azure (Resource Group, VM, etc.) de façon modulaire, en utilisant le SDK Azure .NET 2.x (non Fluent).  
Chaque étape (création du groupe, de la VM, etc.) est encapsulée dans une méthode réutilisable.

## Fonctionnement

- **Authentification** : Utilise `DefaultAzureCredential` pour s’authentifier auprès d’Azure.
- **Création d’un Resource Group** : La méthode `CreateResourceGroupAsync` crée ou met à jour un groupe de ressources dans la région souhaitée.
- **Création d’une VM** : La méthode `CreateVirtualMachineAsync` crée une VM Windows 10 avec les paramètres fournis (nom, admin, NIC, etc.).
- **Extensibilité** : Vous pouvez ajouter d’autres méthodes pour créer les ressources réseau nécessaires (VNet, Subnet, IP publique, NIC).

## Bonnes pratiques

- Les identifiants et secrets ne doivent jamais être stockés en clair dans le code.
- Utilisez des identités managées ou Azure Key Vault pour la gestion des secrets.
- Toutes les opérations sont asynchrones (`async/await`).
- Chaque étape est factorisée pour la réutilisabilité et la sécurité.

## Références de code

- **AzureInfrastructureService.cs** : Service C# pour la gestion modulaire de l’infrastructure Azure.
- [Documentation Azure SDK .NET](https://learn.microsoft.com/fr-fr/dotnet/azure/sdk/)
- [Exemples Azure.ResourceManager](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/resourcemanager)

---

# Infrastructure Azure : création modulaire avec Azure SDK .NET 2.x

## Objectif

Automatiser la création d’objets Azure (Resource Groups, Machines Virtuelles, etc.) depuis l’application, en utilisant le SDK Azure .NET (version 2.x, non Fluent).

## Prérequis

- Installer les packages NuGet suivants dans le projet concerné (exemple pour un projet Console ou Service) :
  - `Azure.Identity`
  - `Azure.ResourceManager`
  - `Azure.ResourceManager.Compute`
  - `Azure.ResourceManager.Resources`
- Disposer d’un compte Azure avec les droits nécessaires.
- Authentification recommandée : DefaultAzureCredential (gère Azure CLI, Managed Identity, etc.).

## Étapes modulaires

### 1. Authentification et initialisation du client

```csharp
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

var credential = new DefaultAzureCredential();
var armClient = new ArmClient(credential, "<subscription-id>");
```

### 2. Création d’un Resource Group

```csharp
var rgCollection = armClient.GetDefaultSubscription().GetResourceGroups();
string rgName = "myResourceGroup";
string location = "westeurope";
var rgData = new ResourceGroupData(location);
var rgLro = await rgCollection.CreateOrUpdateAsync(WaitUntil.Completed, rgName, rgData);
var resourceGroup = rgLro.Value;
```

### 3. Création d’une machine virtuelle

```csharp
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;

// Prérequis : créer un VNet, un subnet, une IP publique, une NIC (voir doc Azure)
// Exemple simplifié pour la VM :
var vmCollection = resourceGroup.GetVirtualMachines();
var vmData = new VirtualMachineData(location)
{
    HardwareProfile = new HardwareProfile { VmSize = VirtualMachineSizeType.StandardD2V3 },
    StorageProfile = new StorageProfile
    {
        OSDisk = new OSDisk(DiskCreateOptionType.FromImage)
        {
            Name = "osdisk",
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
        ComputerName = "vm-demo",
        AdminUsername = "azureuser",
        AdminPassword = "MotDePasseSecurise123!"
    },
    NetworkProfile = new NetworkProfile
    {
        NetworkInterfaces = {
            new NetworkInterfaceReference { Id = "<nic-resource-id>", Primary = true }
        }
    }
};
var vmLro = await vmCollection.CreateOrUpdateAsync(WaitUntil.Completed, "vm-demo", vmData);
var vm = vmLro.Value;
```

### 4. Modularité

- Chaque étape (création du groupe, du réseau, de la VM) doit être encapsulée dans une méthode/service réutilisable.
- Utilisez des paramètres pour rendre les méthodes génériques (nom, région, taille, image, etc.).
- Gérez les exceptions et les statuts d’opération (LRO).

## Bonnes pratiques

- Ne stockez jamais de secrets ou mots de passe en clair dans le code.
- Utilisez des identités managées ou Azure Key Vault pour la gestion des secrets.
- Privilégiez l’asynchrone (`async/await`) pour toutes les opérations Azure.
- Logguez les opérations et surveillez les statuts de déploiement.

## Références

- [Documentation officielle Azure SDK .NET](https://learn.microsoft.com/fr-fr/dotnet/azure/sdk/)
- [Exemples Azure.ResourceManager](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/resourcemanager)

---

**Résumé** :  
La création d’objets Azure (Resource Group, VM, etc.) se fait de façon modulaire via le SDK Azure .NET 2.x, sans Fluent API, en utilisant des clients typés, des modèles de données et des opérations asynchrones. Chaque étape doit être factorisée pour la réutilisabilité et la sécurité.