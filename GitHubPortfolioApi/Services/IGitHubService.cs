using GitHubPortfolioApi.Models;

namespace GitHubPortfolioApi.Services
{
    public interface IGitHubService
    {
        Task<List<GitHubRepoDto>> GetPortfolioAsync();
        Task<List<SearchRepositoryDto>> SearchRepositoriesAsync(string? name = null, string? language = null, string? user = null);
        Task<DateTimeOffset?> GetLastUserActivityAsync();
    }
}
