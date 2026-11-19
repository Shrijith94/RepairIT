# RepairIT – Version 1

Application de vente et de réparation d'ordinateurs en microservices ASP.NET Core (.NET 10).
Module ALTN71 – EFREI 2026-2027.

## Contenu de la V1

- Création de la solution `RepairIT.slnx` et des projets (front et APIs)
- CRUD des ordinateurs (`RepairIT.CatalogService`)
- Gestion des utilisateurs : création (`RepairIT.UserService`). Keycloak n'est pas géré en V1 : le champ `UserIdKeycloak` prend la valeur par défaut `"non-associe"`
- EF Core avec une base SQLite commune à tous les services (projet `RepairIT.Data`) et des migrations
- Application des migrations et données initiales au démarrage
- Tests unitaires des services (xUnit, FluentAssertions, base EF Core en mémoire)
- Injection des dépendances, logging et configuration (`appsettings.json`)

## Projets

| Projet | Rôle | URL (profil http) |
|---|---|---|
| `RepairIT.Client` | Front Blazor WebAssembly (template, intégré en V4) | http://localhost:5228 |
| `RepairIT.CatalogService` | API CRUD ordinateurs | http://localhost:5234/swagger |
| `RepairIT.UserService` | API utilisateurs | http://localhost:5005/swagger |
| `RepairIT.RepairService` | API réparations (template, V2) | http://localhost:5259 |
| `RepairIT.SalesService` | API ventes (template, V3) | http://localhost:5089 |
| `RepairIT.AuthentificationService` | API authentification (template, V5) | http://localhost:5006 |
| `RepairIT.Shared` | Modèles, enums, DTO et options partagés | – |
| `RepairIT.Data` | `RepairItDbContext` commun et migrations EF Core | – |
| `RepairIT.Tests` | Tests unitaires | – |

Dans chaque projet, le code se trouve dans un dossier `src/` (par exemple `RepairIT.CatalogService/src/Controllers`).

## Lancement

```bat
lancer-repairit.bat
```

Le script compile la solution et lance chaque projet dans sa propre fenêtre. Les API se testent avec Swagger.

Pour les tests : `dotnet test RepairIT.slnx`

## Choix techniques

- **Organisation d'une API** : un *Controller* (HTTP) appelle un *Service* (métier + logs) qui utilise le *DbContext* EF Core. Le service est injecté par son interface (`IOrdinateurService`, `IUtilisateurService`), ce qui permet de le remplacer par un mock dans les tests.
- **Injection des dépendances** : tout est enregistré dans `Program.cs` (`AddDbContext`, `AddScoped`).
- **Configuration** : chaîne de connexion dans `ConnectionStrings`, options d'initialisation dans la section `Database` (classe `DatabaseOptions`).
- **Logging** : `ILogger<T>` dans les services (création, modification, suppression, cas introuvables).
- **Initialisation de la base** : au démarrage, `Database.Migrate()` applique les migrations en attente (et crée la base si besoin), puis des données de démonstration sont insérées si la table est vide.
- **Validation** : annotations sur les DTO (`[Required]`, `[Range]`, `[EmailAddress]`). Une requête invalide renvoie une erreur 400.
- **Une base commune** : tous les services utilisent `RepairItDbContext` (projet `RepairIT.Data`) et le même fichier `RepairIT.Data/repairit.db`.

```json
"ConnectionStrings": { "RepairItDb": "Data Source=../RepairIT.Data/repairit.db" },
"Database": {
  "RecreerAuDemarrage": false,
  "InsererDonneesInitiales": true
}
```

## Migrations (EF Core)

Outil à installer une fois : `dotnet tool install --global dotnet-ef`

Les commandes se lancent depuis le dossier `RepairIT.Data` (la classe `RepairItDbContextFactory` fournit le contexte à l'outil) :

```bat
cd RepairIT.Data
dotnet ef migrations add NomDeLaMigration --output-dir src/Migrations
dotnet ef database update
```

Après une modification d'un modèle ou de `RepairItDbContext`, il suffit d'ajouter une migration : elle sera appliquée automatiquement au prochain démarrage d'une API.

## Modèles de données

### Table `Ordinateurs` (CatalogService)

| Colonne | Type | Remarque |
|---|---|---|
| Id | int | Clé primaire |
| Nom | string(100) | Obligatoire |
| Marque | string(50) | Obligatoire |
| Processeur | string(100) | Obligatoire |
| RamGo | int | |
| StockageGo | int | |
| Type | string | Portable, Fixe, Serveur |
| Etat | string | Neuf, Reconditionne, Occasion |
| Prix | decimal(10,2) | |
| QuantiteEnStock | int | |
| DateAjout | DateTime (UTC) | Fixée à la création |

### Table `Utilisateurs` (UserService)

| Colonne | Type | Remarque |
|---|---|---|
| Id | int | Identifiant technique |
| UserIdKeycloak | string(100) | Valeur par défaut `"non-associe"` en V1 |
| Nom | string(100) | Obligatoire |
| Prenom | string(100) | Obligatoire |
| Email | string(200) | Obligatoire |
| Role | string | Gestionnaire, Administrateur |
| EstActif | bool | `true` à la création |
| DateCreation | DateTime (UTC) | Fixée à la création |

Aucun mot de passe n'est stocké : l'authentification sera gérée par Keycloak.

## Endpoints

**CatalogService**

| Méthode | Route | Réponses |
|---|---|---|
| GET | `/api/ordinateurs` | 200 |
| GET | `/api/ordinateurs/{id}` | 200, 404 |
| POST | `/api/ordinateurs` | 201, 400 |
| PUT | `/api/ordinateurs/{id}` | 200, 400, 404 |
| DELETE | `/api/ordinateurs/{id}` | 204, 404 |

**UserService**

| Méthode | Route | Réponses |
|---|---|---|
| GET | `/api/utilisateurs` | 200 |
| GET | `/api/utilisateurs/{id}` | 200, 404 |
| POST | `/api/utilisateurs` | 201, 400 |

## Tests unitaires

`RepairIT.Tests` teste uniquement les services. Chaque test utilise sa propre base EF Core en mémoire (`UseInMemoryDatabase(Guid.NewGuid().ToString())`) :
- `OrdinateurServiceTests` : création, liste triée par nom, lecture, modification, suppression, cas introuvables
- `UtilisateurServiceTests` : création (actif, valeur Keycloak par défaut, rôle), lecture, liste triée par nom, cas introuvable
