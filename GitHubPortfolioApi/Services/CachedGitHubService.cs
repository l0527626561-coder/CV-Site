using GitHubPortfolioApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GitHubPortfolioApi.Services
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _inner;
        private readonly IMemoryCache _cache;
        private const string PortfolioCacheKey = "portfolio_cache";
        private const string LastActivityCacheKey = "last_activity_cache";

        public CachedGitHubService(IGitHubService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<List<GitHubRepoDto>> GetPortfolioAsync()
        {
            var currentActivity = await _inner.GetLastUserActivityAsync();

            if (_cache.TryGetValue(LastActivityCacheKey, out DateTimeOffset? cachedActivity) &&
                _cache.TryGetValue(PortfolioCacheKey, out List<GitHubRepoDto>? cachedPortfolio))
            {
                if (cachedActivity == currentActivity && cachedPortfolio != null)
                {
                    return cachedPortfolio;
                }
            }

            var portfolio = await _inner.GetPortfolioAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(PortfolioCacheKey, portfolio, cacheOptions);
            _cache.Set(LastActivityCacheKey, currentActivity, cacheOptions);

            return portfolio;
        }

        public Task<List<SearchRepositoryDto>> SearchRepositoriesAsync(string? name = null, string? language = null, string? user = null)
        {
            return _inner.SearchRepositoriesAsync(name, language, user);
        }

        public Task<DateTimeOffset?> GetLastUserActivityAsync()
        {
            return _inner.GetLastUserActivityAsync();
        }
    }
}
