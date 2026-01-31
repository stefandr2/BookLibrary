# Copilot instructions for BookLibrary

## Project overview
- Single ASP.NET Core Web API in BookLibrary.Api using EF Core + SQL Server/LocalDB.
- API surface is in Controllers; there is no separate service layer. Controllers work directly with `LibraryContext`.
- Data model centers on Book, Borrower, Rental with relationships configured in `LibraryContext`.

## Architecture and data flow
- App startup in `Program.cs` registers controllers, Swagger (dev only), and `LibraryContext` using `DefaultConnection` from appsettings.
- DB is created and seeded on startup via `DbInitializer.InitializeAsync` (uses EnsureCreated + sample data). For production, prefer migrations.
- Rentals workflow: `RentalsController` sets `Book.IsAvailable` false on rent and true on return; `ReturnDate` drives `Rental.IsActive`.
- Rentals queries use `Include` for `Book` and `Borrower` navigation properties.

## Key files and patterns
- Controllers: `BookLibrary.Api/Controllers/*Controller.cs` with standard CRUD actions and route `api/[controller]`.
- EF Core context and relationships: `BookLibrary.Api/Data/LibraryContext.cs`.
- Seed data: `BookLibrary.Api/Data/DbInitializer.cs` (sample books, borrowers, one active rental).
- Models: `BookLibrary.Api/Models/*.cs` (simple POCOs, navigation lists, `Rental.IsActive` is `[NotMapped]`).
- Migrations live in `BookLibrary.Api/Migrations/`.

## Developer workflows
- Run locally from `BookLibrary.Api`: `dotnet restore`, `dotnet run` (Swagger available in Development).
- EF migrations (optional): `dotnet tool install --global dotnet-ef`, then `dotnet ef migrations add <Name>` and `dotnet ef database update`.

## Conventions to follow
- Keep controllers thin and use `LibraryContext` directly (matches existing style).
- When adding entities, update `LibraryContext` DbSets + relationships, add a migration, and (if needed) extend `DbInitializer` seed data.
- When updating rentals logic, preserve availability toggling and active-only filtering semantics.

## Integration points
- SQL Server connection string in `BookLibrary.Api/appsettings.json` (`DefaultConnection`).
- Local dev uses LocalDB; Azure SQL deployment guidance is in `BookLibrary.Api/README.md`.
