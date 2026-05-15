# Task 02: Add Search Endpoint

## Goal
Add an endpoint to search students by name.

## Endpoint
GET /api/students/search?name=

## Behavior
- Return students where firstName or lastName contains the query
- Case insensitive
- Return empty list if no results

## Constraints
- Do not introduce database
- Use existing in-memory list
- Keep Minimal API style