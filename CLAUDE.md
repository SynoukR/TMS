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

## Charte graphique

### Couleurs
- **Couleur dominante** : bleu clair — utilisée pour les backgrounds, la navbar, la sidebar, les éléments de structure
- **Couleur secondaire** : orange — utilisée pour les boutons d'action, les hover, les CTA, tout ce qui invite à interagir
- Variables CSS définies dans `TMS.Web/src/styles.scss` (`:root`)

### Principes UI
- Interface **simple et épurée** : pas d'éléments décoratifs inutiles, chaque élément a un rôle clair
- **Accessible à tous les types d'utilisateurs** : libellés explicites, hiérarchie visuelle claire, pas de jargon technique dans l'UI
- **Optimisée pour la donnée** : le projet affiche beaucoup d'informations — privilegier les tableaux lisibles, les grilles aérées, une typographie claire et des contrastes suffisants
- Boutons et interactions en **orange** (hover, focus, actions principales)
- Fonds et structure en **bleu** (navbar, sidebar, badges, états actifs)
- Toujours privilégier la **lisibilité** sur l'esthétique : taille de texte suffisante, espacement généreux, pas de surcharge visuelle

### Règles concrètes
- Boutons d'action → orange (`var(--orange)`)
- Hover sur éléments interactifs → orange ou bleu clair selon le contexte
- Backgrounds de page → bleu très clair (`var(--bg-app)`)
- Tableaux : alternance de lignes, en-têtes bien distincts, données alignées
- Pas de plus de 2-3 couleurs par page (bleu + orange + blanc/gris)

## Conventions

- La logique métier va dans `TMS.Core/Services/`
- Les controllers dans `TMS.API` doivent rester fins (pas de logique métier)
- Les appels HTTP Angular vont dans `TMS.Web/src/app/services/`
