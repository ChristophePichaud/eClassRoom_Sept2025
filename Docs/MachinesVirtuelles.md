# Documentation de la page MachinesVirtuelles.razor

## Rôle de la page

La page `MachinesVirtuelles.razor` permet de gérer les machines virtuelles (affichage, création, modification, suppression) via une interface utilisateur Blazor.

## Dépendances principales

- **HttpClient** : Pour les appels HTTP (GET, POST, PUT, DELETE) vers l’API REST `/machines`.
- **MachineVirtuelleDto** (projet Shared) : Sert à transporter les données des machines virtuelles entre le client et le serveur.
- **BootstrapBlazor** : Pour l’UI (tableaux, formulaires).
- **IJSRuntime** : Peut être utilisé pour des interactions avancées côté client.

## Fonctionnement et interactions

1. **Chargement des machines virtuelles**
   - Appel GET `/machines` pour récupérer la liste.
   - Affichage dans un tableau.

2. **Création**
   - Bouton "Nouvelle machine virtuelle" ouvre le formulaire.
   - POST `/machines` avec le DTO pour créer une VM.

3. **Modification**
   - Bouton "Modifier" ouvre le formulaire prérempli.
   - PUT `/machines/{id}` avec le DTO pour modifier.

4. **Suppression**
   - Bouton "Supprimer" appelle DELETE `/machines/{id}`.

## Interactions Services et EF

- **Côté client** :  
  - Utilise uniquement les DTO et l’API REST.
- **Côté serveur** :  
  - Le contrôleur `/machines` reçoit les requêtes, valide et délègue à la couche de services (`MachineVirtuelleService`).
  - La couche de services utilise Entity Framework pour manipuler les entités `MachineVirtuelle` dans la base PostgreSQL.
  - Les entités EF ne sont jamais exposées directement : seules les données des DTO transitent.

## Architecture côté serveur

- **MachinesController** : Contrôleur Web API qui expose les endpoints REST pour la gestion des machines virtuelles. Il reçoit les requêtes du client, les valide et appelle la couche de service.
- **MachineVirtuelleService** : Service métier qui centralise la logique de gestion des machines virtuelles. Il utilise le `EClassRoomDbContext` (Entity Framework) pour accéder à la base de données et effectuer les opérations CRUD sur les entités `MachineVirtuelle`.

## Résumé du flux

1. L’utilisateur interagit avec la page (affichage, ajout, édition, suppression).
2. Le code-behind effectue des appels HTTP vers l’API REST.
3. Le serveur traite via le contrôleur et la couche de services, qui utilise EF pour la base de données.
4. Les réponses sont renvoyées sous forme de DTO et affichées côté client.

## Références de code

- **MachinesVirtuelles.razor** : UI et composants.
- **MachinesVirtuelles.razor.cs** : Logique métier côté client, appels API.
- **MachinesController.cs** (Server) : Endpoints REST pour la gestion des machines virtuelles.
- **Services/MachineVirtuelleService.cs** (Server) : Logique métier et accès aux données via EF.
- **EFModel/Models/MachineVirtuelle.cs** : Entité EF.
- **Shared/MachineVirtuelleDto.cs** : DTO utilisé pour le transport des données.
