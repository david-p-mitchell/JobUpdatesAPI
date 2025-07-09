namespace JobUpdatesAPI.Models;

public class KeywordModel
{
    public int KeywordId { get; set; }
    public required string Keyword { get; set; }

    public List<JobKeywordModel> JobKeywords { get; set; } = [];
}
