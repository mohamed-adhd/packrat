var builder = WebApplication.CreateSlimBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();
app.MapGet("/", () => Results.Json(new { state = "online" }));
app.MapGet("/health", () => Results.Json(new { msg = "fully functional" }));

app.MapGet("/api/users/{id}", (int id) => Results.Json(new { id }));
app.MapPost("/api/users", (User u) => Results.Created($"/api/users/{u.Name}", u));

app.Run();

record User(string Name);
