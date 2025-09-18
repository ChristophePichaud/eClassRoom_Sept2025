# Provisionnement Azure d'une salle de formation – Documentation technique

## Fonctionnement

- Le service `AzureSalleDeFormationService` orchestre la création d'une salle de formation Azure.
- Pour chaque stagiaire, il provisionne une VM, une IP publique, une NIC, et stocke l'adresse IP publique en base.
- Les informations sont persistées dans la table `ProvisionningVM` (EF).
- La méthode `ProvisionnerSalleAsync` retourne un objet `ProvisionningResultDto` contenant la salle et la liste des IPs publiques par stagiaire.

## Flux technique

1. Création du Resource Group, VNet, Subnet pour la salle.
2. Pour chaque stagiaire :
   - Création d'une IP publique, d'une NIC, puis d'une VM.
   - Récupération de l'adresse IP publique.
   - Ajout d'un enregistrement dans la table `ProvisionningVM`.
3. Retour d'un DTO avec la salle et la liste des IPs publiques.

## Modèle EF

- **ProvisionningVM** : table de liaison entre salle, stagiaire, VM et IP publique.
- Ajout du DbSet dans `EClassRoomDbContext` et configuration des relations.

## DTO

- **ProvisionningResultDto** : contient la salle et la liste des stagiaires avec leur IP publique.

## Sécurité

- L'appel à Azure se fait avec un compte technique sécurisé.
- Les informations sensibles (IP, VM) sont stockées côté serveur et transmises uniquement aux utilisateurs autorisés.

---

**Références de code** :
- `AzureSalleDeFormationService.cs`
- `AzureInfrastructureService.cs`
- `ProvisionningVM.cs` (EF)
- `ProvisionningResultDto.cs` (DTO)
