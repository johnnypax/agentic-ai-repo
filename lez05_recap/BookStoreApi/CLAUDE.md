# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Run the API
dotnet run

# Run with a specific profile (http or https)
dotnet run --launch-profile http
dotnet run --launch-profile https

# Build
dotnet build

# Watch mode (auto-restart on file changes)
dotnet watch run
```

The API runs at `http://localhost:5247` (http) or `https://localhost:7245` (https).

## Architecture

This is a minimal ASP.NET Core 9 Web API with no external dependencies and no database. It uses a layered Service/Repository pattern with ASP.NET Core's built-in DI.

**Folder structure**:
```
BookStoreApi/
├── Models/
│   └── Book.cs               — Book model (Id, Title, Author, Year)
├── Repositories/
│   ├── IBookRepository.cs    — data-access interface
│   └── BookRepository.cs     — in-memory List<Book> implementation
├── Services/
│   ├── IBookService.cs       — business-logic interface
│   └── BookService.cs        — delegates to IBookRepository
└── Program.cs                — DI registration + endpoint mapping
```

**Data**: `BookRepository` holds an in-memory `List<Book>` registered as a **Singleton** — data resets on every restart.

**DI registrations** (in `Program.cs`):
- `AddSingleton<IBookRepository, BookRepository>()`
- `AddScoped<IBookService, BookService>()`

**Model**: `Book` lives in `Models/Book.cs` with four properties: `Id`, `Title`, `Author`, `Year`.

**Endpoints**:
- `GET /books` — returns all books
- `GET /books/{id}` — returns one book or 404
- `POST /books` — appends a new book, auto-assigns `Id` as `max(existing ids) + 1`
- `PUT /books/{id}` — updates `Title`, `Author`, `Year` of an existing book; returns 200 or 404
- `DELETE /books/{id}` — deletes the book with `Id`; returns 204 or 404

There are no authentication, middleware, or Swagger/OpenAPI layers configured. The project targets .NET 9 with nullable reference types and implicit usings enabled.

## Constraints

This is a C# Minimal API used for educational videos.
Keep the code simple and progressive.
Do not introduce Entity Framework, authentication, Docker, or complex architecture unless explicitly requested.

Preferred conventions:
- Use clear and readable C# code.
- Explain changes before applying them.
- Prefer small incremental refactorings.
- When adding features, propose a short plan first.
- Keep examples suitable for beginners learning APIs and Claude Code.

