namespace GitHubPortfolioApi.Models
{
    public class GitHubRepoDto
    {
        public string Name { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Dictionary<string, long> Languages { get; set; } = new();
        public DateTimeOffset? LastCommitDate { get; set; }
        public int StarsCount { get; set; }
        public int PullRequestsCount { get; set; }
        public string? Homepage { get; set; }
    }

    public class SearchRepositoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Owner { get; set; } = string.Empty;
        public int StarsCount { get; set; }
        public string? Language { get; set; }
    }
}
