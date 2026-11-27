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
    public async Task<res?> login(string email, string password)
    {
        var body = new creds(email,password);

        var response = await _http.PostAsJsonAsync("/login", body);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<res>();
    } 
}