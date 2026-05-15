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

This is a minimal ASP.NET Core 9 Web API with no external dependencies, no database, and no separate controller or service layers. Everything lives in a single file: `Program.cs`.

**Data**: An in-memory `List<Book>` acts as the data store — data resets on every restart.

**Model**: `Book` is defined at the bottom of `Program.cs` as a top-level class with four properties: `Id`, `Title`, `Author`, `Year`.

**Endpoints**:
- `GET /books` — returns all books
- `GET /books/{id}` — returns one book or 404
- `POST /books` — appends a new book, auto-assigns `Id` as `max(existing ids) + 1`

There are no authentication, middleware, or Swagger/OpenAPI layers configured. The project targets .NET 9 with nullable reference types and implicit usings enabled.
