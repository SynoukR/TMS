# TMS — Terminal Management System

## Description

## Architecture

Le projet suit une architecture **Clean Architecture** en 4 couches :

| Projet | Rôle |
|--------|------|
| `TMS.Core` | Domaine métier : entités, interfaces, services |
| `TMS.Infrastructure` | Accès aux données : EF Core, repositories, PostgreSQL |
| `TMS.API` | API REST ASP.NET Core |
| `TMS.Web` | Frontend Angular |

## Prérequis

- [.NET 10](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [PostgreSQL 17](https://www.postgresql.org/download/)

## Installation

### 1. Base de données

Créer une base PostgreSQL et renseigner la connection string dans `TMS.API/appsettings.Development.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tms_db;Username=postgres;Password=yourpassword"
  }
}
```

### 2. Migrations

```bash
cd TMS.API
dotnet ef database update --project ../TMS.Infrastructure/TMS.Infrastructure.csproj --startup-project TMS.API.csproj
```

### 3. API

```bash
cd TMS.API
dotnet run
```

L'API démarre sur `http://localhost:5182`.

### 4. Frontend

```bash
cd TMS.Web
npm install
npm start
```

Le frontend démarre sur `http://localhost:4200`.
