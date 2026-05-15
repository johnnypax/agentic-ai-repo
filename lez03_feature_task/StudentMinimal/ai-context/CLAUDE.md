# CLAUDE.md

## Goal

This project is a C# Minimal API built with .NET 9.
The goal is to create a small but clean REST API for managing students.

## Technology Stack

- C#
- .NET 9
- ASP.NET Core Minimal API
- In-memory storage for the first version
- Swagger/OpenAPI for endpoint documentation

## Coding rules
- Keep the code simple and readable.
- Use meaningful names.
- Avoid overengineering.
- Do not introduce Clean Architecture yet.
- Do not create controllers.
- Do not add Entity Framework Core in this version.
- Do not add a database in this version.
- Prefer small methods and clear responsibilities.

## API Rules
- Use REST conventions.
- Use plural resource names.
- Use `/api/students` as the base route.
- Return proper HTTP status codes.
- Validate input before inserting or updating data.

## Validation rules
- FirstName: required
- LastName: required
- Email: required and valid enough for demo purposes
- Age: must be greater than 0

## C# rules
- Use records or classes where appropriate.
- Use nullable reference types correctly.
- Prefer explicit DTOs for requests.
- Avoid putting too much logic directly inside endpoint lambdas.
- Extract helper methods when the endpoint becomes too long.

## Output rules
When modifying code:
1. Explain the planned changes before editing.
2. Modify only the necessary files.
3. After the change, suggest the command to run.
4. Do not invent dependencies unless required.