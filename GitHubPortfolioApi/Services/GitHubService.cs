using GitHubPortfolioApi.Models;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubPortfolioApi.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly string _username;

        public GitHubService(IOptions<GitHubOptions> options)
        {
            var opts = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _username = opts.Username;

            _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolioApi"));
            var tokenAuth = new Credentials(opts.Token);
            _client.Credentials = tokenAuth;
        }

        public async Task<List<GitHubRepoDto>> GetPortfolioAsync()
        {
            var repos = await _client.Repository.GetAllForUser(_username);
            var result = new List<GitHubRepoDto>();

            foreach (var repo in repos)
            {
                var dto = new GitHubRepoDto
                {
                    Name = repo.Name,
                    HtmlUrl = repo.HtmlUrl,
                    Description = repo.Description,
                    StarsCount = repo.StargazersCount,
                    Homepage = repo.Homepage
                };

                var languages = await _client.Repository.GetAllLanguages(repo.Id);
                dto.Languages = languages.ToDictionary(l => l.Name, l => l.NumberOfBytes);

                var commits = await _client.Repository.Commit.GetAll(repo.Id, new ApiOptions { PageSize = 1, PageCount = 1 });
                if (commits.Any())
                {
                    dto.LastCommitDate = commits[0].Commit.Author.Date;
                }

                var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Id);
                dto.PullRequestsCount = pullRequests.Count;

                result.Add(dto);
            }

            return result;
        }

        public async Task<List<SearchRepositoryDto>> SearchRepositoriesAsync(string? name = null, string? language = null, string? user = null)
        {
            var searchTerms = new List<string>();

            if (!string.IsNullOrWhiteSpace(name))
                searchTerms.Add(name);

            if (!string.IsNullOrWhiteSpace(language))
                searchTerms.Add($"language:{language}");

            if (!string.IsNullOrWhiteSpace(user))
                searchTerms.Add($"user:{user}");

            if (!searchTerms.Any())
                searchTerms.Add("stars:>0");

            var searchRequest = new SearchRepositoriesRequest(string.Join(" ", searchTerms));
            var searchResult = await _client.Search.SearchRepo(searchRequest);

            return searchResult.Items.Select(repo => new SearchRepositoryDto
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Description = repo.Description,
                Owner = repo.Owner.Login,
                StarsCount = repo.StargazersCount,
                Language = repo.Language
            }).ToList();
        }

        public async Task<DateTimeOffset?> GetLastUserActivityAsync()
        {
            var events = await _client.Activity.Events.GetAllUserPerformed(_username, new ApiOptions { PageSize = 1, PageCount = 1 });
            return events.FirstOrDefault()?.CreatedAt;
        }
    }
}
