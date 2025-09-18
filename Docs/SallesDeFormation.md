# Documentation de la page SallesDeFormation.razor

## Rôle de la page

La page `SallesDeFormation.razor` permet de gérer les salles de formation (affichage, création, modification, suppression) via une interface utilisateur Blazor.

## Dépendances principales

- **HttpClient** : Pour les appels HTTP (GET, POST, PUT, DELETE) vers l’API REST `/salles`.
- **SalleDeFormationDto** (projet Shared) : Sert à transporter les données des salles entre le client et le serveur.
- **BootstrapBlazor** : Pour l’UI (tableaux, formulaires).
- **IJSRuntime** : Peut être utilisé pour des interactions avancées côté client.

## Fonctionnement et interactions

1. **Chargement des salles**
   - Appel GET `/salles` pour récupérer la liste.
   - Affichage dans un tableau.

2. **Création**
   - Bouton "Nouvelle salle de formation" ouvre le formulaire.
   - POST `/salles` avec le DTO pour créer une salle.

3. **Modification**
   - Bouton "Modifier" ouvre le formulaire prérempli.
   - PUT `/salles/{id}` avec le DTO pour modifier.

4. **Suppression**
   - Bouton "Supprimer" appelle DELETE `/salles/{id}`.

## Interactions Services et EF

- **Côté client** :  
  - Utilise uniquement les DTO et l’API REST.
- **Côté serveur** :  
  - Le contrôleur `/salles` reçoit les requêtes, valide et délègue à la couche de services (`SalleDeFormationService`).
  - La couche de services utilise Entity Framework pour manipuler les entités `SalleDeFormation` dans la base PostgreSQL.
  - Les entités EF ne sont jamais exposées directement : seules les données des DTO transitent.

## Architecture côté serveur

- **SallesController** : Contrôleur Web API qui expose les endpoints REST pour la gestion des salles de formation. Il reçoit les requêtes du client, les valide et appelle la couche de service.
- **SalleDeFormationService** : Service métier qui centralise la logique de gestion des salles. Il utilise le `EClassRoomDbContext` (Entity Framework) pour accéder à la base de données et effectuer les opérations CRUD sur les entités `SalleDeFormation`.

## Résumé du flux

1. L’utilisateur interagit avec la page (affichage, ajout, édition, suppression).
2. Le code-behind effectue des appels HTTP vers l’API REST.
3. Le serveur traite via le contrôleur et la couche de services, qui utilise EF pour la base de données.
4. Les réponses sont renvoyées sous forme de DTO et affichées côté client.

## Références de code

- **SallesDeFormation.razor** : UI et composants.
- **SallesDeFormation.razor.cs** : Logique métier côté client, appels API.
- **SallesController.cs** (Server) : Endpoints REST pour la gestion des salles.
- **Services/SalleDeFormationService.cs** (Server) : Logique métier et accès aux données via EF.
- **EFModel/Models/SalleDeFormation.cs** : Entité EF.
- **Shared/SalleDeFormationDto.cs** : DTO utilisé pour le transport des données.
