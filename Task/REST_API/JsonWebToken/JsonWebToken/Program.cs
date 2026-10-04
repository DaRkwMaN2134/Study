using DataLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
    });
}

var todos = new List<Todo>
{
    new(1, "Первая", false),
    new(2, "Вторая", true)
};


app.MapGet("/todos", () => Results.Ok(todos));

app.MapGet("/todos/{id}", (string id) =>
{
    int.TryParse(id, out int intId);
    var todo = todos.FirstOrDefault(i => i.Id == intId);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.MapPost("/todos", (Todo todo) =>
{
    todo.Id = todos.Max(i => i.Id) + 1;
    if (string.IsNullOrWhiteSpace(todo.Title))
    {
        return Results.BadRequest("Title не может быть пустым");
    }
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", (string id, Todo todo) =>
{
    int.TryParse(id, out int intId);
    var existing = todos.FirstOrDefault(i => i.Id == intId);

    if (existing is null) return Results.NotFound();
    if (string.IsNullOrWhiteSpace(todo.Title)) return Results.BadRequest();

    existing.Title = todo.Title;
    existing.IsDone = todo.IsDone;
    return Results.Ok(existing);
});

app.MapPatch("/todos/{id}/done", (string id) =>
{
    int.TryParse(id, out int intId);
    var todo = todos.FirstOrDefault(i => i.Id == intId);
    if (todo is null) return Results.NotFound();
    todo.IsDone = true;
    return Results.Ok(todo);
});

app.MapDelete("/todos/{id}", (string id) =>
{
    int.TryParse(id, out int intId);
    var todo = todos.FirstOrDefault(i => i.Id == intId);
    if (todo is null) return Results.NotFound();
    todos.Remove(todo);
    return Results.NoContent();
});

app.UseHttpsRedirection();
app.Run();