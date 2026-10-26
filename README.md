# .NET Task API Starter

A small, portfolio-ready ASP.NET Core Web API example for managing tasks. This repository is a learning/demo project, not a client deliverable.

## What it demonstrates
- REST endpoints with clear HTTP status codes
- Request validation and predictable error responses
- Dependency injection and separation of concerns
- Swagger/OpenAPI for exploring the API
- In-memory storage for a simple local demo (data resets when the app restarts)

## Requirements
- .NET 8 SDK (or update the target framework to an installed supported version)

## Run locally
```bash
dotnet restore
dotnet run
```
Open the Swagger URL printed in the terminal.

## Example endpoints
- `GET /api/tasks` — list tasks
- `GET /api/tasks/{id}` — retrieve one task
- `POST /api/tasks` — create a task
- `PUT /api/tasks/{id}` — update a task
- `DELETE /api/tasks/{id}` — delete a task

## Note
This demo intentionally uses in-memory data. For a production implementation, add a persistent database, authentication/authorization, structured logging, integration tests, and environment-specific configuration. Never commit secrets or connection strings.
