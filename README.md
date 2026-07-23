# Student Management System

A Student Management System REST API built with ASP.NET Core, using Entity Framework Core (SQL Server), JWT authentication, Serilog logging, and Swagger for API documentation.

## Tech Stack

- **.NET 8.0** (ASP.NET Core Web API)
- **Entity Framework Core 8** with **SQL Server**
- **JWT Bearer Authentication**
- **Serilog** (console + file logging)
- **Swashbuckle / Swagger** for API docs

## Project Structure

```
├── Controllers/     # API endpoints
├── Data/             # DbContext and database setup
├── Middleware/        # Custom middleware
├── Migrations/        # EF Core migrations
├── Models/            # Entity/domain models
├── Repositories/       # Data access layer
├── Services/           # Business logic layer
├── Program.cs          # App entry point / configuration
├── appsettings.json    # App configuration
└── StudentManagementSystem.csproj
```

## Prerequisites

Before you begin, make sure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB, SQL Server Express, or full SQL Server)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet) (for running migrations)
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- A code editor such as [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Setup Steps

### 1. Clone the repository

```bash
git clone https://github.com/ganeshsuryawansh/Student-Management.git
cd Student-Management
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the database connection

Open `appsettings.json` (or better, use `appsettings.Development.json` / user secrets for local dev) and update the `ConnectionStrings:DefaultConnection` value to match your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StudentManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Adjust `Server=` to point at your own SQL Server instance (e.g. `localhost`, `(localdb)\\MSSQLLocalDB`, or a remote server).

> **Security note:** `appsettings.json` in this repo also contains a sample JWT signing key and a default admin username/password. These are placeholder dev values — **do not use them in production**. Replace `Jwt:Key`, `AdminUser:Username`, and `AdminUser:Password` with your own secrets, ideally via environment variables or [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) rather than committing them to source control.

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "your-own-long-random-secret-key"
dotnet user-secrets set "AdminUser:Password" "your-own-admin-password"
```

### 4. Apply database migrations

This will create the database and schema defined in the `Migrations/` folder:

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

By default the API will be available at the URLs shown in the console output (typically something like `https://localhost:5001` and `http://localhost:5000` — check the console for the exact ports).

### 6. Explore the API

With the app running, open the Swagger UI in your browser to view and test the available endpoints:

```
https://localhost:<port>/swagger
```

### 7. Authenticate

The API uses JWT bearer authentication. Use the seeded admin credentials (from `AdminUser` in `appsettings.json`, or your own configured values) to log in via the auth endpoint, then include the returned token in the `Authorization: Bearer <token>` header for protected requests.

## Running Migrations After Model Changes

If you modify any entity models, generate and apply a new migration:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Logging

Logs are written via Serilog to both the console and log files (see `Program.cs` for sink configuration). Check the configured log output directory for file-based logs when running locally.

## Contributing

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes
4. Push to your branch and open a Pull Request

## License

No license specified yet — add one (e.g. MIT) if you intend this repo to be used/reused by others.