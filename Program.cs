using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/tasks", (ITaskRepository repo) => Results.Ok(repo.GetAll()))
   .WithName("GetTasks").WithOpenApi();
app.MapGet("/api/tasks/{id:int}", (int id, ITaskRepository repo) =>
    repo.Get(id) is { } task ? Results.Ok(task) : Results.NotFound())
   .WithName("GetTask").WithOpenApi();
app.MapPost("/api/tasks", ([FromBody] CreateTaskRequest request, ITaskRepository repo) =>
{
    var title = request.Title?.Trim();
    if (string.IsNullOrWhiteSpace(title))
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = ["Title is required."] });
    if (title.Length > 100)
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = ["Title must be 100 characters or fewer."] });
    var created = repo.Create(title);
    return Results.Created($"/api/tasks/{created.Id}", created);
}).WithName("CreateTask").WithOpenApi();
app.MapPut("/api/tasks/{id:int}", (int id, [FromBody] UpdateTaskRequest request, ITaskRepository repo) =>
{
    var title = request.Title?.Trim();
    if (string.IsNullOrWhiteSpace(title))
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = ["Title is required."] });
    if (title.Length > 100)
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = ["Title must be 100 characters or fewer."] });
    var updated = repo.Update(id, title, request.IsComplete);
    return updated is null ? Results.NotFound() : Results.Ok(updated);
}).WithName("UpdateTask").WithOpenApi();
app.MapDelete("/api/tasks/{id:int}", (int id, ITaskRepository repo) =>
    repo.Delete(id) ? Results.NoContent() : Results.NotFound())
   .WithName("DeleteTask").WithOpenApi();

app.Run();

public record TaskItem(int Id, string Title, bool IsComplete, DateTimeOffset CreatedAt);
public record CreateTaskRequest(string? Title);
public record UpdateTaskRequest(string? Title, bool IsComplete);

public interface ITaskRepository
{
    IReadOnlyCollection<TaskItem> GetAll();
    TaskItem? Get(int id);
    TaskItem Create(string title);
    TaskItem? Update(int id, string title, bool isComplete);
    bool Delete(int id);
}
public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _items = new();
    private int _nextId;
    public InMemoryTaskRepository()
    {
        Create("Review API validation");
        Create("Add integration tests");
    }
    public IReadOnlyCollection<TaskItem> GetAll() => _items.Values.OrderBy(x => x.Id).ToArray();
    public TaskItem? Get(int id) => _items.GetValueOrDefault(id);
    public TaskItem Create(string title)
    {
        var id = Interlocked.Increment(ref _nextId);
        var item = new TaskItem(id, title, false, DateTimeOffset.UtcNow);
        _items[id] = item;
        return item;
    }
    public TaskItem? Update(int id, string title, bool isComplete)
    {
        if (!_items.ContainsKey(id)) return null;
        var item = _items.AddOrUpdate(id, _ => new TaskItem(id, title, isComplete, DateTimeOffset.UtcNow),
            (_, old) => old with { Title = title, IsComplete = isComplete });
        return item;
    }
    public bool Delete(int id) => _items.TryRemove(id, out _);
}
