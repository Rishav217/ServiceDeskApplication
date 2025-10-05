# Service Desk Application

An ASP.NET Core Web API for an IT/internal service desk ticketing system, with JWT-based authentication and role-aware access to tickets, priorities, statuses, and areas.

## Features

- JWT authentication (login/issue token via `AuthController`)
- Ticket creation, assignment, and lifecycle tracking
- Priorities, statuses, and areas as configurable lookups
- Dashboard endpoint for summary/reporting views
- User management

## Architecture

```
Service Desk Application
├── Controllers   Auth, Tickets, Users, Priorities, Statuses, Areas, Dashboard
├── Services      JwtService (token issuance/validation)
├── DTOs          Request/response contracts (Auth, Ticket, Common)
├── Data          EF Core DbContext (SQL Server)
└── Models        Domain entities + DB views (VwActive*)
```

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core (SQL Server / LocalDB)
- JWT Bearer authentication

## Getting Started

```bash
git clone https://github.com/Rishav217/ServiceDeskApplication.git
cd ServiceDeskApplication/"Service Desk Application"
```

Update `appsettings.json`:
- `ConnectionStrings:DefaultConnection` — point at your SQL Server/LocalDB instance
- `Jwt:Key` — replace the placeholder with your own secret (use User Secrets or environment variables in real deployments, never commit a real key)

```bash
dotnet run
```

API docs available via `Service Desk Application.http` once running.
