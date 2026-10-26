using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new
{
    name = "Task API",
    message = "Demo API is running.",
    endpoints = new[] { "GET /api/tasks", "GET /api/tasks/{id}", "POST /api/tasks", "PUT /api/tasks/{id}", "DELETE /api/tasks/{id}" }
}));

var tasks = new ConcurrentDictionary<int, TaskItem>();
var nextId = 0;

app.MapGet("/api/tasks", () => Results.Ok(tasks.Values.OrderBy(task => task.Id)));

app.MapGet("/api/tasks/{id:int}", (int id) =>
    tasks.TryGetValue(id, out var task) ? Results.Ok(task) : Results.NotFound());

app.MapPost("/api/tasks", (CreateTaskRequest request) =>
{
    var title = request.Title?.Trim();
    if (string.IsNullOrWhiteSpace(title))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = new[] { "A task title is required." }
        });

    if (title.Length > 120)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = new[] { "The title must be 120 characters or fewer." }
        });

    var id = Interlocked.Increment(ref nextId);
    var task = new TaskItem(id, title, false);
    tasks[id] = task;
    return Results.Created($"/api/tasks/{id}", task);
});

app.MapPut("/api/tasks/{id:int}", (int id, UpdateTaskRequest request) =>
{
    if (!tasks.TryGetValue(id, out var current))
        return Results.NotFound();

    var title = request.Title?.Trim();
    if (string.IsNullOrWhiteSpace(title))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = new[] { "A task title is required." }
        });

    if (title.Length > 120)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = new[] { "The title must be 120 characters or fewer." }
        });

    var updated = new TaskItem(id, title, request.Completed);
    tasks[id] = updated;
    return Results.Ok(updated);
});

app.MapDelete("/api/tasks/{id:int}", (int id) =>
    tasks.TryRemove(id, out _) ? Results.NoContent() : Results.NotFound());

app.Run();

record TaskItem(int Id, string Title, bool Completed);
record CreateTaskRequest(string? Title);
record UpdateTaskRequest(string? Title, bool Completed);
