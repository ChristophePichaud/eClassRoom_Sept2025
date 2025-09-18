L’application repose sur une architecture **multi-couches** :
- **Client** : Application Blazor WebAssembly pour l’interface utilisateur.
- **Server** : API REST .NET (Web API) pour l’accès aux données et la logique métier.
- **EFModel** : Modèles de données Entity Framework Core et accès à la base PostgreSQL.
- **Shared** : Objets de transfert de données (DTO) partagés entre le client et le serveur.

Les pages razor sont organisées dans le dossier `Pages`.
Chaque page copntient son code C# dans un fichier code-behind (ex : `Clients.razor.cs`).
Les pages Razor utilisent des composants BootstrapBlazor pour l’UI.
Les appels HTTP vers l’API REST du serveur se font via `HttpClient`.
Le projet `Server` expose des endpoints REST (contrôleurs Web API) :
Chaque contrôleur reçoit les requêtes du client, les valide et appelle la couche de services.
La couche de services (dossier `Services`) centralise la logique métier et les accès aux données via Entity Framework.
La configuration du serveur (connexion PostgreSQL, CORS, Swagger, etc.) est réalisée dans `Program.cs` et `appsettings.json`.
Le projet `EFModel` regroupe :
Les entités du modèle de données (dossier `Models`) : `Client`, `Utilisateur`, `SalleDeFormation`, `MachineVirtuelle`, `Facture`.
Le contexte de base de données `EClassRoomDbContext` configuré pour PostgreSQL.
Ce projet est référencé par le serveur pour l’accès aux données.
Le projet `Shared` contient les **DTO** (Data Transfer Objects) :
Les DTO servent à échanger des données entre le client et le serveur de façon sécurisée et adaptée.
Ils évitent d’exposer directement les entités EF côté client.
Le **client** envoie des requêtes HTTP (GET, POST, PUT, DELETE) vers les endpoints du **serveur**.
Le **serveur** traite les requêtes via ses contrôleurs, qui délèguent la logique à la couche de services.
La couche de services utilise **Entity Framework** pour interagir avec la base PostgreSQL.
Les données sont échangées sous forme de **DTO** définis dans le projet `Shared`.
Les mots de passe doivent être stockés de façon sécurisée (hashage côté serveur).
Les accès à l’API sont à sécuriser (authentification, autorisation).
L’utilisation de DTO permet de maîtriser les données exposées au client.
L’architecture en couches facilite l’extensibilité et la maintenance de l’application.

A chaque fois que Copilot génère du code, il faut ajouter des explications dans ce fichier pour expliquer le code généré dans le répertoire `Docs`. de haut niveau..

Je voudrais que tu reprennes le code et me génère la documentation dans le folder Docs tel qu'indiquer dans mes instructions.
L'application utilise l’authentification basée sur des tokens JWT pour sécuriser l’accès à l’API REST. Cette approche permet de vérifier l’identité des utilisateurs et de protéger les endpoints sensibles.

## Arborescence de l'application
- **Server** : Génère et valide les tokens JWT. Les contrôleurs REST sont protégés par l’attribut `[Authorize]`.
- **Client** : Récupère le token lors de la connexion et l’ajoute dans l’en-tête `Authorization` des requêtes HTTP.
- **Shared** : Utilise des DTO pour transporter les données d’authentification. 
- **EFModel** : Utilisé pour valider les identifiants côté serveur (vérification du hash du mot de passe).
## Étapes principales
1. L'utilisateur envoie ses identifiants (nom d'utilisateur et mot de passe) au serveur via une requête HTTP POST.
2. Le serveur valide les identifiants en vérifiant le hash du mot de passe.
3. Si les identifiants sont valides, le serveur génère un token JWT et le renvoie au client.
4. Le client stocke le token (par exemple, dans le stockage local) et l'inclut dans l'en-tête `Authorization` des requêtes HTTP ultérieures.
5. Le serveur vérifie le token à chaque requête protégée et autorise ou refuse l'accès en conséquence.
## Points de sécurité
- Les mots de passe sont stockés de façon sécurisée (hashés) côté serveur.
- Le secret JWT doit être complexe et stocké de façon sécurisée (jamais dans le
code source).
- Les tokens ont une durée de vie limitée (ex : 2h).
- Les données sensibles ne transitent jamais en clair.
## Avantages de cette approche
- Séparation claire des responsabilités entre les couches.
- Sécurité renforcée grâce à l’utilisation de JWT et de DTO.
- Facilité d’extension pour la gestion des rôles, des permissions, ou du rafraîchissement de token.
## Références de code
- **Program.cs** : Configuration JWT.
- **appsettings.json** : Paramètres du token.
- **AuthController.cs** : Génération et validation du JWT.
- **ClientsController.cs** : Exemple de protection d’un endpoint.
- **Login.razor.cs** : Récupération et stockage du token côté client.
- **Program.cs (Client)** : Injection automatique du token dans les requêtes HTTP.
## Vue d’ensemble
L’application utilise l’authentification basée sur des tokens JWT pour sécuriser l’accès à l’API REST. Cette approche permet de vérifier l’identité des utilisateurs et de protéger les endpoints sensibles.
## Architecture concernée
- **Server** : Génère et valide les tokens JWT. Les contrôleurs REST sont protégés par l’attribut `[Authorize]`.
- **Client** : Récupère le token lors de la connexion et l’ajoute dans l’en-tête `Authorization` des requêtes HTTP.
- **Shared** : Utilise des DTO pour transporter les données d’authentification.
- **EFModel** : Utilisé pour valider les identifiants côté serveur (vérification du hash du mot de passe).
## Étapes principales
### 1. Configuration du serveur
- Ajout du package NuGet `Microsoft.AspNetCore.Authentication.JwtBearer`.
- Configuration de l’authentification JWT dans `Program.cs` : définition du schéma, des paramètres de validation, et du secret partagé (stocké dans `appsettings.json`).
- Ajout de l’appel à `app.UseAuthentication()` et `app.UseAuthorization()` dans le pipeline.
### 2. Endpoint d’authentification
- Création d’un contrôleur `AuthController` avec une méthode `Login` qui :
  - Valide les identifiants reçus (vérification du hash du mot de passe).
  - Génère un token JWT signé si l’authentification réussit.
  - Retourne le token au client dans la réponse.
### 3. Sécurisation des endpoints
- Ajout de l’attribut `[Authorize]` sur les contrôleurs ou actions à protéger.
- Seuls les clients présentant un JWT valide peuvent accéder à ces endpoints.
### 4. Côté client (Blazor WebAssembly)
- Lors de la connexion, le client envoie les identifiants à l’API `/api/auth/login`.
- Si la connexion réussit, le token JWT est stocké dans le `localStorage` du navigateur.
- Un `DelegatingHandler` personnalisé ajoute automatiquement le token dans l’en-tête `Authorization` (`Bearer {token}`) de chaque requête HTTP vers l’API.
## Points de sécurité
- Les mots de passe sont stockés de façon sécurisée (hashés) côté serveur.
- Le secret JWT doit être complexe et stocké de façon sécurisée (jamais dans le code source).
- Les tokens ont une durée de vie limitée (ex : 2h).
- Les données sensibles ne transitent jamais en clair.
## Avantages de cette approche
- Séparation claire des responsabilités entre les couches.
- Sécurité renforcée grâce à l’utilisation de JWT et de DTO.
- Facilité d’extension pour la gestion des rôles, des permissions, ou du rafraîchissement de token.
## Références de code
- **Program.cs** : Configuration JWT.
- **appsettings.json** : Paramètres du token.
- **AuthController.cs** : Génération et validation du JWT.
- **ClientsController.cs** : Exemple de protection d’un endpoint.
- **Login.razor.cs** : Récupération et stockage du token côté client.
- **Program.cs (Client)** : Injection automatique du token dans les requêtes HTTP.
- **AuthService.cs** : Service pour gérer l’authentification et le stockage du token.
- **HttpClientFactory.cs** : Configuration du client HTTP avec le token.
## Exemple de code
### Configuration JWT dans `appsettings.json`
```json
{
  "Jwt": {
    "Key": "VotreCléSecrète",
    "Issuer": "VotreIssuer",
    "Audience": "VotreAudience",
    "DurationInMinutes": 120
  }
}       


