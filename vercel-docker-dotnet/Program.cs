using Npgsql;
var builder = WebApplication.CreateSlimBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();
app.MapGet("/", () => Results.Json(new { state = "online" }));
var raw = Environment.GetEnvironmentVariable("DATABASE_URL")
builder.Services.AddSingleton(NpgsqlDataSource.Create(ToNpgsql(raw)));



app.MapPost("/login",async(NewItem body,NpgsqlDataSource db)=>{
    await using var cmd = db.CreateCommand("SELECT COUNT(*) FROM users WHERE email = $1 AND password = $2;");
    cmd.Parameters.AddWithValue(body.email);  
    cmd.Parameters.AddWithValue(body.passwd);      
    var rows = await cmd.ExecuteNonQueryAsync();
    return rows == 0;

});








app.Run();

record User(string Name);
