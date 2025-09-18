# Documentation métier : Gestion des machines virtuelles

## 1. Rôle métier des machines virtuelles

Dans eClassRoom, une **machine virtuelle** (VM) représente un environnement informatique individuel provisionné pour un stagiaire ou un formateur dans le cadre d’une salle de formation. Chaque VM est associée à une session de formation et permet d’accéder à un poste de travail distant (Windows ou Linux) avec une configuration adaptée (type, OS, disque, etc.).

Les machines virtuelles sont créées, modifiées ou supprimées par les administrateurs via l’interface web. Elles sont provisionnées dans Azure et associées à des utilisateurs et des salles de formation.

## 2. Attributs métier d’une machine virtuelle

Les principaux attributs d’une VM sont :
- **Id** : Identifiant unique.
- **Nom** : Nom de la machine virtuelle.
- **TypeOs** : Système d’exploitation (Windows, Linux).
- **TypeVm** : Type de VM Azure (ex : Standard_D2s_v3).
- **Sku** : SKU Azure.
- **Offer** : Offre Azure.
- **Version** : Version de l’image.
- **DiskIso** : ISO ou disque utilisé pour l’initialisation.
- **NomMarketing** : Nom commercial ou marketing de la VM.

## 3. DTO (Data Transfer Object)

Le **MachineVirtuelleDto** (dans le projet Shared) permet de transporter les données d’une VM entre le client et le serveur sans exposer directement l’entité EF. Il reprend les attributs métier nécessaires à l’UI et aux échanges API.

## 4. Service métier

Le **MachineVirtuelleService** (dans le dossier Services du Server) centralise la logique métier :
- Récupération de la liste des VMs.
- Création d’une nouvelle VM à partir d’un DTO.
- Modification et suppression d’une VM existante.
- Utilisation du DbContext EF pour toutes les opérations sur la base.

Ce service garantit la cohérence métier et isole la logique d’accès aux données.

## 5. Couche Entity Framework (EF)

L’entité **MachineVirtuelle** (dans EFModel/Models) correspond à la table des machines virtuelles en base PostgreSQL.  
Le **EClassRoomDbContext** expose un DbSet<MachineVirtuelle> pour permettre les opérations CRUD via EF Core.

Les relations éventuelles (ex : association à une salle de formation ou à un utilisateur) sont gérées dans le modèle EF.

## 6. Flux métier

1. L’utilisateur (admin) crée/modifie/supprime une VM via l’UI.
2. Le client Blazor envoie un DTO au serveur via l’API REST.
3. Le contrôleur appelle le service, qui applique la logique métier et utilise EF pour persister les changements.
4. Les données sont échangées sous forme de DTO, jamais d’entités EF.

## 7. Sécurité et bonnes pratiques

- Les endpoints sont protégés par JWT et `[Authorize]`.
- Seuls les administrateurs ou formateurs autorisés peuvent gérer les VMs.
- Les données sensibles (ex : configuration, accès) ne sont jamais exposées directement.

---

**Résumé** :  
La gestion des machines virtuelles dans eClassRoom repose sur une séparation stricte entre la couche métier (service), la couche d’accès aux données (EF), et la couche de présentation (DTO/UI), garantissant sécurité, évolutivité et clarté du modèle métier.
