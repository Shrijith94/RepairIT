# RepairIT – Version 1

Application de vente et de réparation d'ordinateurs en microservices ASP.NET Core (.NET 10).
Module ALTN71 – EFREI 2026-2027.

## Contenu de la V1

- Création de la solution `RepairIT.slnx` et des projets (front et APIs)
- CRUD des ordinateurs (`RepairIT.CatalogService`)
- Gestion des utilisateurs : création (`RepairIT.UserService`). Le champ `UserIdKeycloak` prend la valeur par défaut `"non-associe"` pour le moment
- EF Core avec une base SQLite par microservice (`CatalogService` et `UserService`), chacune avec ses propres migrations
- Application des migrations et données initiales au démarrage
- Tests unitaires des services qui reprensentent la partie métier de l'application (xUnit, FluentAssertions, base EF Core en mémoire)
- Injection des dépendances, logging et configuration (`appsettings.json`)

## Projets

| Projet | Rôle | URL (profil http) |
|---|---|---|
| `RepairIT.Client` | Front Blazor WebAssembly| http://localhost:5228 |
| `RepairIT.CatalogService` | API CRUD ordinateurs | http://localhost:5234/swagger |
| `RepairIT.UserService` | API utilisateurs | http://localhost:5005/swagger |
| `RepairIT.RepairService` | API réparations | http://localhost:5259 |
| `RepairIT.SalesService` | API ventes | http://localhost:5089 |
| `RepairIT.AuthentificationService` | API authentification | http://localhost:5006 |
| `RepairIT.Shared` | Modèles, enums, DTO et options partagés | – |
| `RepairIT.Tests` | Tests unitaires | – |

Dans chaque projet, le code se trouve dans un dossier `src/` (par exemple `RepairIT.CatalogService/src/Controllers`).

## Lancement

```bat
./repairIT.bat
```

Le script compile la solution et lance chaque projet dans sa propre fenêtre. Les API se testent avec Swagger.

Pour les tests : `dotnet test RepairIT.slnx`

## Choix techniques

- **Organisation d'une API** : un *Controller* (HTTP) appelle un *Service* (métier) qui utilise le *DbContext* EF Core. Le service est injecté par son interface (`IOrdinateurService`, `IUtilisateurService`), ce qui permet de le remplacer par un mock dans les tests.
- **Injection des dépendances** : tout est enregistré dans `Program.cs` (`AddDbContext`, `AddScoped`).
- **Configuration** : chaîne de connexion dans `ConnectionStrings`, options d'initialisation dans la section `Database` (classe `DatabaseOptions`).
- **Logging** : `ILogger<T>` dans les controllers pour gérer les logs API dans la console du terminal.
- **Initialisation de la base** : au démarrage, `Database.Migrate()` applique les migrations en attente (et crée la base si besoin), puis des données de démonstration sont insérées si la table est vide.
- **Une base par microservice** : chaque service a son propre `DbContext` (`CatalogDbContext`, `UserDbContext`) dans `src/Data` et son propre fichier SQLite (`catalog.db`, `users.db`). Un service ne lit jamais la base d'un autre.

```json
"ConnectionStrings": { "CatalogDb": "Data Source=catalog.db" },
"Database": {
  "RecreerAuDemarrage": false,
  "InsererDonneesInitiales": true
}
```

## Migrations (EF Core)

Outil à installer une fois : `dotnet tool install --global dotnet-ef`

Chaque microservice a ses propres migrations (dossier `Migrations` du projet). Les commandes se lancent depuis le dossier du service concerné ; `dotnet ef` lit la chaîne de connexion dans son `appsettings.json` :

```bat
cd RepairIT.CatalogService
dotnet ef migrations add NomDeLaMigration --output-dir Migrations
dotnet ef database update
```

(idem dans `RepairIT.UserService`)

Après une modification d'un modèle ou d'un `DbContext`, il suffit d'ajouter une migration dans le service concerné : elle sera appliquée automatiquement au prochain démarrage de l'API.

## Modèles de données
Pour le moment dans la V1 nous n'avons que deux tables : "Ordinateurs" (base du CatalogService) et "Utilisateurs" (base du UserService). Les autres tables seront ajoutées dans les prochaines versions.

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

`RepairIT.Tests` teste uniquement les services car ils représentent la logique métier (donc uniquement les classe Service présent dans le dossier `Services` sont couverts) de l'application. Chaque test utilise sa propre base EF Core en mémoire (`UseInMemoryDatabase(Guid.NewGuid().ToString())`) :
- `OrdinateurServiceTests` : création, liste triée par nom, lecture, modification, suppression, cas introuvables
- `UtilisateurServiceTests` : création , lecture, liste triée par nom, cas introuvable
