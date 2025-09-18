# Documentation de la page Utilisateurs.razor

## Rôle de la page

La page `Utilisateurs.razor` permet de gérer les utilisateurs (affichage, création, modification, suppression) via une interface utilisateur Blazor.

## Dépendances principales

- **HttpClient** : Pour les appels HTTP (GET, POST, PUT, DELETE) vers l’API REST `/users` et `/clients`.
- **UtilisateurDto** (projet Shared) : Sert à transporter les données des utilisateurs entre le client et le serveur, incluant l’identifiant de la société (ClientId).
- **ClientDto** (projet Shared) : Sert à afficher la liste des sociétés dans la combo box.
- **BootstrapBlazor** : Pour l’UI (tableaux, formulaires, combo box).
- **IJSRuntime** : Peut être utilisé pour des interactions avancées côté client.

## Fonctionnement et interactions

1. **Chargement des utilisateurs et des sociétés**
   - Appel GET `/clients` pour récupérer la liste des sociétés.
   - Appel GET `/users` pour récupérer la liste des utilisateurs.
   - Affichage dans un tableau.

2. **Création et modification**
   - Bouton "Nouvel utilisateur" ou "Modifier" ouvre le formulaire.
   - Le formulaire contient une combo box pour sélectionner la société (ClientId).
   - POST `/users` ou PUT `/users/{id}` avec le DTO incluant le ClientId.

3. **Suppression**
   - Bouton "Supprimer" appelle DELETE `/users/{id}`.

## Interactions Services et EF

- **Côté client** :  
  - Utilise uniquement les DTO et l’API REST.
  - La combo box est alimentée par la liste des clients récupérée via `/clients`.
- **Côté serveur** :  
  - Le contrôleur `/users` reçoit les requêtes, valide et délègue à la couche de services (`UtilisateurService`).
  - La couche de services utilise Entity Framework pour manipuler les entités `Utilisateur` dans la base PostgreSQL, en tenant compte de la relation avec `Client`.
  - Les entités EF ne sont jamais exposées directement : seules les données des DTO transitent.

## Architecture côté serveur

- **UsersController** : Contrôleur Web API qui expose les endpoints REST pour la gestion des utilisateurs. Il reçoit les requêtes du client, les valide et appelle la couche de service.
- **UtilisateurService** : Service métier qui centralise la logique de gestion des utilisateurs. Il utilise le `EClassRoomDbContext` (Entity Framework) pour accéder à la base de données et effectuer les opérations CRUD sur les entités `Utilisateur`, en gérant la relation avec la société.

## Résumé du flux

1. L’utilisateur interagit avec la page (affichage, ajout, édition, suppression).
2. Le code-behind effectue des appels HTTP vers l’API REST.
3. Le serveur traite via le contrôleur et la couche de services, qui utilise EF pour la base de données.
4. Les réponses sont renvoyées sous forme de DTO et affichées côté client.

## Particularité

- Lors de la création ou modification d’un utilisateur, une combo box permet de sélectionner la société à laquelle il appartient (ClientId).

## Références de code

- **Utilisateurs.razor** : UI et composants, combo box pour la société.
- **Utilisateurs.razor.cs** : Logique métier côté client, appels API, gestion de la liste des sociétés.
- **UsersController.cs** (Server) : Endpoints REST pour la gestion des utilisateurs.
- **Services/UtilisateurService.cs** (Server) : Logique métier et accès aux données via EF.
- **EFModel/Models/Utilisateur.cs** : Entité EF (avec clé étrangère vers Client).
- **Shared/UtilisateurDto.cs** : DTO utilisé pour le transport des données, incluant ClientId.
- **Shared/ClientDto.cs** : DTO pour la liste des sociétés.
