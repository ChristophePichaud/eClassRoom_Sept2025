# Documentation métier : Gestion des salles de formation

## 1. Rôle métier des salles de formation

Dans eClassRoom, une **salle de formation** représente un espace virtuel où se déroule une session de formation. Elle regroupe un formateur, des stagiaires, une période (date de début/fin) et les ressources nécessaires (machines virtuelles, etc.).

Les salles sont créées et gérées par les administrateurs ou formateurs via l’interface web.

## 2. Attributs métier d’une salle de formation

Les principaux attributs d’une salle sont :
- **Id** : Identifiant unique.
- **Nom** : Nom de la salle ou de la formation.
- **Formateur** : Nom ou identifiant du formateur responsable.
- **DateDebut** : Date de début de la session.
- **DateFin** : Date de fin de la session.

## 3. DTO (Data Transfer Object)

Le **SalleDeFormationDto** (dans le projet Shared) permet de transporter les données d’une salle entre le client et le serveur sans exposer directement l’entité EF. Il reprend les attributs métier nécessaires à l’UI et aux échanges API.

## 4. Service métier

Le **SalleDeFormationService** (dans le dossier Services du Server) centralise la logique métier :
- Récupération de la liste des salles.
- Création d’une nouvelle salle à partir d’un DTO.
- Modification et suppression d’une salle existante.
- Utilisation du DbContext EF pour toutes les opérations sur la base.

Ce service garantit la cohérence métier et isole la logique d’accès aux données.

## 5. Couche Entity Framework (EF)

L’entité **SalleDeFormation** (dans EFModel/Models) correspond à la table des salles de formation en base PostgreSQL.  
Le **EClassRoomDbContext** expose un DbSet<SalleDeFormation> pour permettre les opérations CRUD via EF Core.

Les relations éventuelles (ex : association à un formateur, à des stagiaires, à des machines virtuelles) sont gérées dans le modèle EF.

## 6. Flux métier

1. L’utilisateur (admin ou formateur) crée/modifie/supprime une salle via l’UI.
2. Le client Blazor envoie un DTO au serveur via l’API REST.
3. Le contrôleur appelle le service, qui applique la logique métier et utilise EF pour persister les changements.
4. Les données sont échangées sous forme de DTO, jamais d’entités EF.

## 7. Sécurité et bonnes pratiques

- Les endpoints sont protégés par JWT et `[Authorize]`.
- Seuls les utilisateurs autorisés peuvent gérer les salles.
- Les données sensibles ne sont jamais exposées directement.

---

**Résumé** :  
La gestion des salles de formation dans eClassRoom repose sur une séparation stricte entre la couche métier (service), la couche d’accès aux données (EF), et la couche de présentation (DTO/UI), garantissant sécurité, évolutivité et clarté du modèle métier.
