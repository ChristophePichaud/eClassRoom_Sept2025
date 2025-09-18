# Documentation de la page Clients.razor

## Rôle de la page

La page `Clients.razor` permet d’afficher, de créer, de modifier et de supprimer des clients dans l’application. Elle constitue l’interface utilisateur pour la gestion des entités `Client`.

## Dépendances principales

- **HttpClient** : Utilisé pour effectuer les appels HTTP (GET, POST, PUT, DELETE) vers l’API REST du serveur pour récupérer et manipuler les données des clients.
- **DTO ClientDto** (projet Shared) : Sert à transporter les données des clients entre le client Blazor et le serveur, sans exposer directement les entités EF.
- **BootstrapBlazor** : Fournit les composants UI pour l’affichage des listes, formulaires et dialogues de confirmation.
- **IJSRuntime** : Peut être utilisé pour des interactions avancées côté client (notifications, confirmations, etc.).

## Fonctionnement et interactions

1. **Chargement des clients**
   - Au chargement de la page, le code-behind appelle l’API (`GET /api/clients`) via `HttpClient` pour récupérer la liste des clients.
   - Les données reçues (liste de `ClientDto`) sont affichées dans un composant BootstrapBlazor (tableau, liste, etc.).

2. **Ajout d’un client**
   - L’utilisateur saisit les informations d’un nouveau client dans un formulaire.
   - Le code-behind envoie une requête `POST` à l’API (`POST /api/clients`) avec le DTO du client à créer.
   - Le serveur ajoute le client via la couche de services, qui utilise Entity Framework pour insérer l’entité dans la base PostgreSQL.

3. **Modification d’un client**
   - L’utilisateur édite un client existant via un formulaire ou un dialogue.
   - Le code-behind envoie une requête `PUT` à l’API (`PUT /api/clients/{id}`) avec le DTO mis à jour.
   - Le serveur met à jour l’entité correspondante via la couche de services et EF.

4. **Suppression d’un client**
   - L’utilisateur confirme la suppression d’un client.
   - Le code-behind envoie une requête `DELETE` à l’API (`DELETE /api/clients/{id}`).
   - Le serveur supprime l’entité via la couche de services et EF.

5. **Sécurité**
   - Les appels à l’API sont protégés par JWT : le token est automatiquement ajouté dans l’en-tête `Authorization` par le handler HTTP.
   - Le contrôleur côté serveur est décoré avec `[Authorize]` pour restreindre l’accès aux utilisateurs authentifiés.

## Interactions avec les services et Entity Framework

- **Côté client** :  
  - `Clients.razor` et son code-behind n’accèdent jamais directement à la base de données ni à EF.
  - Toutes les opérations passent par l’API REST, en utilisant des DTO.

- **Côté serveur** :  
  - Le contrôleur `ClientsController` reçoit les requêtes du client, valide les données, puis délègue la logique à la couche de services (`ClientService`).
  - La couche de services utilise Entity Framework (`EFModel`) pour interagir avec la base PostgreSQL (lecture, création, modification, suppression d’entités `Client`).
  - Les entités EF ne sont jamais exposées directement au client : seules les données des DTO transitent.

## Résumé du flux

1. L’utilisateur interagit avec la page `Clients.razor` (affichage, ajout, édition, suppression).
2. Le code-behind effectue des appels HTTP vers l’API REST du serveur.
3. Le serveur traite les requêtes via le contrôleur et la couche de services, qui utilise EF pour manipuler la base de données.
4. Les réponses sont renvoyées sous forme de DTO et affichées côté client.

## Références de code

- **Clients.razor** : UI et composants BootstrapBlazor.
- **Clients.razor.cs** : Logique métier côté client, appels API, gestion des états.
- **ClientsController.cs** (Server) : Endpoints REST pour la gestion des clients.
- **Services/ClientService.cs** (Server) : Logique métier et accès aux données via EF.
- **EFModel/Models/Client.cs** : Entité EF représentant un client.
- **Shared/ClientDto.cs** : DTO utilisé pour le transport des données client.
