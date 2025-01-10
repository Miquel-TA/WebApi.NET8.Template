# MyAppSolution

This is a .NET 8 Web API solution with a clean, one-project-per-layer DDD structure:

- **Cross**: Shared Models (`User`), and Utils (`JwtUtils`, `BcryptUtils`)
- **Logic**: Business rules, interfaces, and implementations (with dependencies for EF).
- **Repository**: EF Core context and repository. InMemory database by default.
- **Presentation**: ASP.NET Core Web API with Swagger, JWT auth, and seeded `admin` user.

### Key Points

1. **appsettings.json** holds your secrets (admin password, JWT key, issuer). If missing or empty, the app throws an exception.
2. **InMemory** EF for easy local testing. Edit `ServiceRegistration` to switch to SQL Server or another provider.
3. **Random admin password** is stored in the `appsettings.json` automatically. The console prints it at startup.
4. **JWT**: The key and issuer are also in `appsettings.json`. They must not be empty for production.

### Usage

- **`dotnet run`** in `MyApp.Presentation`.
- **Swagger** at `https://localhost:<port>/swagger`.
- `[AllowAnonymous]` login endpoint to get your JWT token.  
- `[Authorize]` endpoints for CRUD on the user repository.

Enjoy your DDD-based Web API template!
