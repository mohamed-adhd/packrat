using Npgsql;
var builder = WebApplication.CreateSlimBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
var raw = Environment.GetEnvironmentVariable("CON_STRING");
builder.Services.AddSingleton(NpgsqlDataSource.Create(ToNpgsql(raw)));

var app = builder.Build();

app.MapGet("/", () => Results.Json(new { state = "online" }));




app.MapPost("/login", async (LoginRequest body, NpgsqlDataSource db) =>
{
    await using var cmd = db.CreateCommand(
        "SELECT COUNT(*) FROM users WHERE email = $1 AND password = $2");
    cmd.Parameters.AddWithValue(body.email);
    cmd.Parameters.AddWithValue(body.password);

    var count = (long)(await cmd.ExecuteScalarAsync())!;
    return count > 0
        ? Results.Ok(new { pass = true })
        : Results.Unauthorized();
});







app.Run();

static string ToNpgsql(string url)
{
    if (!url.StartsWith("postgres")) return url;
    var u = new Uri(url);
    var user = u.UserInfo.Split(':', 2);
    return new NpgsqlConnectionStringBuilder
    {
        Host = u.Host,
        Port = u.Port > 0 ? u.Port : 5432,
        Database = u.AbsolutePath.TrimStart('/'),
        Username = Uri.UnescapeDataString(user[0]),
        Password = Uri.UnescapeDataString(user[1]),
        SslMode = SslMode.Require
    }.ConnectionString;
}

record LoginRequest(string email, string password);
