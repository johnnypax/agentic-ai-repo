# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
dotnet build                        # Build the project
dotnet run                          # Run on http://localhost:5247
dotnet run --launch-profile https   # Run on https://localhost:7245
```

No test project is configured.

## Architecture

This is a .NET 9 ASP.NET Core **Minimal API** project. The entire application lives in a single file:

- **`Program.cs`** — entry point, in-memory data store (a `List<Book>`), and all route handlers
- **`Book` model** — defined inline in `Program.cs` with `Id`, `Title`, `Author`, `Year`

### Endpoints

**Books**

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/books` | Return all books |
| GET | `/books/{id}` | Return one book by ID, 404 if missing |
| POST | `/books` | Add a new book; ID is auto-generated as `max(existing ids) + 1` |

**Users**

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/users` | Return all users |
| GET | `/users/{id}` | Return one user by ID, 404 if missing |
| POST | `/users` | Add a new user (`Name`, `Email`); ID is auto-generated |

### Data persistence

There is no database. Data is held in `List<Book>` and `List<User>` initialized at startup with hardcoded seed entries. All state is lost on restart.

### Extending the API

When adding endpoints, follow the existing Minimal API style (`app.MapGet`, `app.MapPost`, etc.) directly in `Program.cs`. If the project grows to need separation of concerns, prefer route groups (`app.MapGroup`) or endpoint extension methods before introducing controllers.
