namespace JobUpdatesAPI.Models;

public class JobKeywordModel
{
    public int JobId { get; set; }
    public int KeywordId { get; set; }

    public JobModel? Job { get; set; }
    public KeywordModel? Keyword { get; set; }
}
