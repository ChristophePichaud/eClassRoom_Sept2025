# Documentation métier : Gestion des utilisateurs

## 1. Rôle métier des utilisateurs

Dans eClassRoom, un **utilisateur** représente toute personne ayant accès à la plateforme : administrateur, formateur ou stagiaire. Chaque utilisateur possède un profil, un rôle et des accès adaptés à ses fonctions.

Les utilisateurs sont créés et gérés par les administrateurs via l’interface web.

## 2. Attributs métier d’un utilisateur

Les principaux attributs d’un utilisateur sont :
- **Id** : Identifiant unique.
- **Email** : Adresse email (identifiant de connexion).
- **Nom** : Nom de famille.
- **Prénom** : Prénom.
- **MotDePasse** : Mot de passe (stocké hashé côté serveur).
- **Role** : Rôle de l’utilisateur (admin, formateur, stagiaire).

## 3. DTO (Data Transfer Object)

Le **UtilisateurDto** (dans le projet Shared) permet de transporter les données d’un utilisateur entre le client et le serveur sans exposer directement l’entité EF. Il reprend les attributs métier nécessaires à l’UI et aux échanges API.

## 4. Service métier

Le **UtilisateurService** (dans le dossier Services du Server) centralise la logique métier :
- Récupération de la liste des utilisateurs.
- Création d’un nouvel utilisateur à partir d’un DTO.
- Modification et suppression d’un utilisateur existant.
- Utilisation du DbContext EF pour toutes les opérations sur la base.

Ce service garantit la cohérence métier et isole la logique d’accès aux données.

## 5. Couche Entity Framework (EF)

L’entité **Utilisateur** (dans EFModel/Models) correspond à la table des utilisateurs en base PostgreSQL.  
Le **EClassRoomDbContext** expose un DbSet<Utilisateur> pour permettre les opérations CRUD via EF Core.

Les relations éventuelles (ex : association à un client, à une salle, à des machines virtuelles) sont gérées dans le modèle EF.

## 6. Flux métier

1. L’administrateur crée/modifie/supprime un utilisateur via l’UI.
2. Le client Blazor envoie un DTO au serveur via l’API REST.
3. Le contrôleur appelle le service, qui applique la logique métier et utilise EF pour persister les changements.
4. Les données sont échangées sous forme de DTO, jamais d’entités EF.

## 7. Sécurité et bonnes pratiques

- Les endpoints sont protégés par JWT et `[Authorize]`.
- Seuls les administrateurs peuvent gérer les utilisateurs.
- Les mots de passe sont stockés de façon sécurisée (hashés).
- Les données sensibles ne sont jamais exposées directement.

---

**Résumé** :  
La gestion des utilisateurs dans eClassRoom repose sur une séparation stricte entre la couche métier (service), la couche d’accès aux données (EF), et la couche de présentation (DTO/UI), garantissant sécurité, évolutivité et clarté du modèle métier.
