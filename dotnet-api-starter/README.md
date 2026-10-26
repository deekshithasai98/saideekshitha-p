# Task API Starter

A small ASP.NET Core minimal API demonstrating common REST endpoints and basic request validation.

## Requirements

- .NET 8 SDK
- An editor or terminal

## Run it

```bash
dotnet restore
dotnet run
```

Open the local URL printed by the app. In Development, `/swagger` provides an interactive API page.

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | `/api/tasks` | List tasks |
| GET | `/api/tasks/{id}` | Get one task |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task |

Example request body for `POST /api/tasks`:

```json
{ "title": "Review accessibility findings" }
```

## Notes

The sample keeps data in memory. Records disappear when the app stops, and this is not a production-ready persistence layer. A next iteration could add EF Core, SQL Server, automated tests, logging, and authentication.
