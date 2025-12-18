using GitHubPortfolioApi;
using GitHubPortfolioApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHub"));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<GitHubService>();
builder.Services.AddScoped<IGitHubService>(provider =>
{
    var gitHubService = provider.GetRequiredService<GitHubService>();
    var cache = provider.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
    return new CachedGitHubService(gitHubService, cache);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("Welcome to the GitHub Portfolio API!"))
   .WithName("Root")
   .WithOpenApi();

app.MapGet("/api/portfolio", async (IGitHubService gitHubService) =>
{
    try
    {
        var portfolio = await gitHubService.GetPortfolioAsync();
        return Results.Ok(portfolio);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error fetching portfolio: {ex.Message}");
    }
})
.WithName("GetPortfolio")
.WithOpenApi();

app.MapGet("/api/search", async (IGitHubService gitHubService, string? name, string? language, string? user) =>
{
    try
    {
        var results = await gitHubService.SearchRepositoriesAsync(name, language, user);
        return Results.Ok(results);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error searching repositories: {ex.Message}");
    }
})
.WithName("SearchRepositories")
.WithOpenApi();

app.Run();
