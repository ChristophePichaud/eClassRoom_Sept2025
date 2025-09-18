# Documentation de la page Login.razor

## Rôle de la page

La page `Login.razor` permet à l’utilisateur de s’authentifier dans l’application. Elle présente un formulaire de connexion (nom d’utilisateur et mot de passe) et gère l’envoi des identifiants au serveur pour obtenir un token JWT.

## Dépendances principales

- **HttpClient** : Utilisé pour envoyer la requête POST d’authentification à l’API REST du serveur (`/api/auth/login`).
- **IJSRuntime** : Permet de stocker le token JWT dans le `localStorage` du navigateur côté client.
- **DTO LoginDto** (projet Shared) : Sert à transporter les identifiants de connexion du client vers le serveur.
- **TokenResponse** : Objet utilisé pour désérialiser la réponse contenant le token JWT.

## Fonctionnement et interactions

1. **Affichage du formulaire**  
   La page Razor affiche un formulaire BootstrapBlazor pour saisir le nom d’utilisateur et le mot de passe.

2. **Soumission du formulaire**  
   Lors de la soumission, la méthode `HandleLogin()` du code-behind est appelée.

3. **Appel à l’API d’authentification**  
   - `HttpClient.PostAsJsonAsync` envoie les identifiants au serveur.
   - Le serveur (contrôleur `AuthController`) reçoit la requête, valide les identifiants via la couche de services (qui utilise EF pour accéder à la table des utilisateurs et vérifier le hash du mot de passe).
   - Si la validation réussit, le serveur génère un JWT et le retourne dans la réponse.

4. **Stockage du token côté client**  
   - Le code-behind récupère le token JWT de la réponse.
   - Il utilise `IJSRuntime` pour stocker ce token dans le `localStorage` du navigateur.

5. **Utilisation du token pour les requêtes suivantes**  
   - Un `DelegatingHandler` personnalisé injecte automatiquement le token JWT dans l’en-tête `Authorization` de chaque requête HTTP vers l’API.
   - Cela permet d’accéder aux endpoints protégés par `[Authorize]` côté serveur.

## Interactions avec les services et Entity Framework

- **Côté serveur** :  
  - Le contrôleur `AuthController` délègue la validation des identifiants à un service d’authentification (dans le dossier `Services`).
  - Ce service utilise Entity Framework (`EFModel`) pour accéder à la base PostgreSQL, rechercher l’utilisateur et comparer le hash du mot de passe.
  - Si la validation est positive, le service génère un JWT.

- **Côté client** :  
  - La page `Login.razor` n’accède pas directement à la base de données ni à EF : elle interagit uniquement avec l’API via `HttpClient` et gère le stockage du token.

## Sécurité

- Les mots de passe ne transitent jamais en clair : seul le hash est comparé côté serveur.
- Le token JWT est stocké côté client et transmis dans l’en-tête `Authorization` pour chaque requête protégée.

## Résumé du flux

1. L’utilisateur saisit ses identifiants sur `Login.razor`.
2. Le code-behind envoie ces identifiants à l’API d’authentification.
3. Le serveur valide via EF et, si OK, retourne un JWT.
4. Le client stocke le JWT et l’utilise pour les requêtes futures.

## Références de code

- **Login.razor** : Formulaire et UI.
- **Login.razor.cs** : Logique de soumission, appel API, gestion du token.
- **AuthController.cs** (Server) : Endpoint d’authentification.
- **Services/AuthService.cs** (Server) : Validation des identifiants via EF.
- **EFModel/Models/Utilisateur.cs** : Entité utilisateur pour la validation.
