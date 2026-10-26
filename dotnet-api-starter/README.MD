# Task API Starter

A small ASP.NET Core Web API demonstrating REST endpoints and basic request validation.

## Technologies
- C#
- ASP.NET Core Minimal API
- .NET 8
- Swagger / OpenAPI

## Run locally

Install the .NET 8 SDK, then open a terminal in this project folder and run:

bash
dotnet restore
dotnet run


Use the local URL printed in the terminal. In Development mode, open /swagger to explore the API.

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | /api/tasks | List tasks |
| GET | /api/tasks/{id} | Get one task |
| POST | /api/tasks | Create a task |
| PUT | /api/tasks/{id} | Update a task |
| DELETE | /api/tasks/{id} | Delete a task |

Example request body:

json
{
  "title": "Review accessibility findings"
}


## Notes

This demo stores tasks in memory, so data resets when the application restarts. It is a learning project, not a production-ready service
