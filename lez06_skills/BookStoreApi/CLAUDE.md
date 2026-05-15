# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```powershell
# Run the API
dotnet run

# Build
dotnet build

# Watch mode (auto-reload on changes)
dotnet watch run
```

The API starts on `http://localhost:5247` (HTTP) or `https://localhost:7245` (HTTPS).

## Architecture

This is a minimal ASP.NET Core 9 Web API using the **Minimal APIs** pattern — no controllers, no services, no separate files. Everything lives in `Program.cs`:

- **Data layer**: an in-memory `List<Book>` acting as a fake database (data is lost on restart).
- **Model**: `Book` record class defined at the bottom of `Program.cs` (`Id`, `Title`, `Author`, `Year`).
- **Endpoints**:
  - `GET /books` — returns all books
  - `GET /books/{id}` — returns one book or 404
  - `POST /books` — inserts a book, auto-generates `Id`, returns 201

No authentication, no persistence, no middleware beyond defaults. The project targets `.NET 9` with nullable reference types and implicit usings enabled.
