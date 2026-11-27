namespace packrat.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Threading.Tasks;
public record creds(string email, string password);

public record res(bool pass);
public class ApiClient
{
    private readonly HttpClient _http = new()
    {
        BaseAddress = new Uri("https://packrat-git-main-midouamdouni4-7219s-projects.vercel.app/")
    };
    public async Task<bool> login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("/login", new creds(email, password));
        Console.WriteLine($"login -> {(int)response.StatusCode}");
        return response.IsSuccessStatusCode;
    }
}