# MyAppBenchAssignment

A small .NET 10 Razor Pages employee management application using EF Core and local SQL Server.

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB (included with Visual Studio) or another local SQL Server instance

## Run locally

From the project directory:

```powershell
dotnet restore
dotnet run
```

The default development connection string is in `appsettings.json` and uses:

```text
(localdb)\mssqllocaldb / MyAppBenchAssignment
```

To use another SQL Server instance, set `ConnectionStrings:DefaultConnection` through user secrets or an environment-specific configuration file. Do not commit credentials.

The application creates the database and seeds two sample employees when running in the Development environment. Initialization is non-destructive: existing employee data is not deleted or replaced.

## Verify the application

- Open the URL printed by `dotnet run`.
- Select **Manage employees**.
- Create, edit, and delete an employee.
- Check the health endpoint at `/healthz`; it should return HTTP 200 with a status of `ok`.

The project currently uses SQL Server for local development. The EF Core provider and connection configuration are isolated so the deployment version can switch to PostgreSQL for AWS managed PostgreSQL.
