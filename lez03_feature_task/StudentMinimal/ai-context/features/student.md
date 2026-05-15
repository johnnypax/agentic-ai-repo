# Feature: Students API

## Goal

Create a Minimal API module to manage students.

## Endpoints

The API must expose the following endpoints:

- GET `/api/students`
- GET `/api/students/{id}`
- POST `/api/students`
- PUT `/api/students/{id}`
- DELETE `/api/students/{id}`

## Student Model

- Id: integer
- FirstName: string
- LastName: string
- Email: string
- Age: integer

## Create Student Request

Fields:

- FirstName
- LastName
- Email
- Age

## Update Student Request

Fields:

- FirstName
- LastName
- Email
- Age

## Behavior

- GET all returns all students.
- GET by id returns 404 if the student does not exist.
- POST validates the request and returns 201 Created.
- PUT validates the request and returns 404 if the student does not exist.
- DELETE returns 404 if the student does not exist.
- DELETE returns 204 No Content when successful.
                                                                            
## Constraints
- Use in-memory storage.
- Do not use a real database.
- Keep everything understandable for a beginner/intermediate C# developer.