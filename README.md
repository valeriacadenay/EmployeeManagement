# EmployeeManagement

Clean architecture ASP.NET Core solution hosting:

| Project | Purpose |
|---|---|
| `Management.Domain` | Domain entities and shared primitives. |
| `Management.Application` | Application services, interfaces and DTOs. |
| `Management.Infrastructure` | Data access (EF Core/PostgreSQL), Identity, JWT, SMTP email, and portal orchestration. |
| `Management.Api` | Public REST API used by employees/clients. |
| `Admin.Web` | MVC administration portal with Identity-protected UI.

## Prerequisites

1. [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.
2. Docker Engine + Compose (optional but recommended) to run PostgreSQL in `db` service described in `compose.yaml`.
3. A PostgreSQL database ready to accept `Host=db;Port=5432;Database=employee_db;Username=employee_admin;Password=EmployeeAdmin_2025!` (the container exposes the DB internally at `db:5432`).

## Environment variables

Set the following (see `.env` for reference):

- `ConnectionStrings__DefaultConnection` – `Host=db;Port=5432;Database=employee_db;Username=employee_admin;Password=EmployeeAdmin_2025!`
- `JWT_ISSUER`, `JWT_AUDIENCE`, `JWT_SECRET` – values must match the `Jwt` section consumed by both API and Admin (secret ≥32 chars).
- `EmailSettings__*` – configure SMTP host, port, credentials and `From` address for welcome mails.

### Admin.Web-specific variables

- `ConnectionStrings__DefaultConnection` – same value as the API so the admin portal shares the same database.
- `Jwt__Issuer`, `Jwt__Audience`, `Jwt__Secret` – must match the API settings so Identity and JWT middleware remain aligned.
- `EmailSettings__*` – reuse SMTP configuration when the admin UI sends notifications (optional).

## Running the stack

### Using Docker Compose (recommended)

```bash
docker compose up -d db
docker compose exec api dotnet run --project Management.Api/Management.Api.csproj
```

The API container uses the same configuration it will read in production: migrations run automatically on startup, Identity roles/users are seeded, and the admin user (`admin@management.com` / `Admin123`) exists.

### Local .NET execution

```bash
dotnet restore Management.Api/Management.Api.csproj
dotnet run --project Management.Api/Management.Api.csproj
```

Make sure `.env` or your shell exports the `ConnectionStrings__DefaultConnection` and JWT values before running.

## Authentication flows

1. **Login** – `POST /api/auth/login` accepts `{ "email", "password" }` and returns `AuthResponse` containing a `token`.  
2. **Register** – `POST /api/auth/register` accepts an `EmployeeRegistrationDto` payload, creates the Domain `Employee`, Identity user, assigns the `Employee` role, and dispatches a welcome email through the configured `IEmailSender`.  
3. **JWT usage** – Use `Authorization: Bearer <token>` for protected endpoints. Tokens are signed with `JwtSettings.SecretKey` (≥32 chars), and audiences/issuers are shared between the JWT generator and middleware.

## Protecting `/api/Employees`

This controller is `[ApiController]` + `[Route("api/[controller]")]` and decorated with `[Authorize(Roles = $"{SystemRoles.Administrator},{SystemRoles.Employee}")]`. Unauthenticated requests receive `401` + `WWW-Authenticate: Bearer`, so always log in first. Example flow:

```bash
curl -X POST http://localhost:5254/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@management.com","password":"Admin123"}'

curl -X GET http://localhost:5254/api/Employees \
  -H "Authorization: Bearer <TOKEN_FROM_LOGIN>" \
  -H "Accept: application/json"
```

Any `accept` header is fine; the prior `401` was due to the missing token, not the media type.

## SMTP welcome emails

Configure `EmailSettings` either via `appsettings.json`, `appsettings.Development.json`, or environment variables (e.g., `EmailSettings__SmtpHost`, `EmailSettings__SmtpPort`, `EmailSettings__User`, `EmailSettings__Password`, `EmailSettings__UseSsl`, `EmailSettings__From`).

During registration the API logs warnings if SMTP settings are incomplete or the provider fails, but it still returns success while logging the failure reason. Provide valid credentials to send real emails.

## Troubleshooting

- Ensure migrations are applied (`dotnet ef database update` or let the app run with migration on startup).  
- Confirm PostgreSQL container `db` is healthy before running API.  
- If NuGet restore fails due to network restrictions, wait until feeds are reachable and retry `dotnet restore`.  
- Logs from `SmtpEmailSender` and `EmployeePortalService` surface delivery failures without interrupting registration.



Clone this repository to your computer https://github.com/valeriacadenay/EmployeeManagement

Run the project

## 👩‍💻 Author
- Valeria Cadena Yance 
- Clan: Caiman
