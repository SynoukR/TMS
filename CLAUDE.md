# TMS — Instructions pour Claude

## Stack technique

- **Backend** : ASP.NET Core (.NET 10), C#
- **ORM** : Entity Framework Core + Npgsql (PostgreSQL)
- **Frontend** : Angular 21 (TypeScript)
- **Base de données** : PostgreSQL 17

## Architecture

Clean Architecture en 4 projets :

- `TMS.Core` — entités, interfaces, services métier (pas de dépendances externes)
- `TMS.Infrastructure` — implémentation EF Core, repositories
- `TMS.API` — controllers ASP.NET Core, Program.cs
- `TMS.Web` — frontend Angular (`src/app/`)

## Lancer le projet en local

- PostgreSQL tourne en service Windows (`postgresql-x64-17`)
- Credentials locaux dans `TMS.API/appsettings.Development.json` (gitignored)
- API : `cd TMS.API && dotnet run` → `http://localhost:5182`
- Frontend : `cd TMS.Web && npm start` → `http://localhost:4200`
- Migrations : `dotnet ef database update --project ../TMS.Infrastructure --startup-project .` depuis `TMS.API/`

## Conventions

- La logique métier va dans `TMS.Core/Services/`
- Les controllers dans `TMS.API` doivent rester fins (pas de logique métier)
- Les appels HTTP Angular vont dans `TMS.Web/src/app/services/`
