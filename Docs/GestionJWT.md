# Gestion de l’authentification JWT (JSON Web Token)

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

