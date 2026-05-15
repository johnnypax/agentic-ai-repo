# Task 01: Create Students Minimal API

## Goal
Implement the Students API described in `ai-context/features/student.md`.

## Files to inspect
- `Program.cs`
- `CLAUDE.md`
- `ai-context/features/student.md`

## Implementation steps
1. Inspect the current project structure.
2. Add the Student model.
3. Add request DTOs.
4. Update `Program.cs`.
5. Add in-memory data storage.
6. Add CRUD endpoints.
7. Add basic validation.
8. Keep Swagger enabled.
9. Provide the command to run the application.

## Acceptance Criteria
- The project builds successfully.
- Swagger shows all students endpoints.
- POST rejects invalid input.
- GET by id returns 404 for missing students.
- PUT updates an existing student.
- DELETE removes an existing student.