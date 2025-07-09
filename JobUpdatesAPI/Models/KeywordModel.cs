using System.ComponentModel.DataAnnotations;

namespace JobUpdatesAPI.Models;

public class KeywordModel
{
    [Key]
    public int KeywordId { get; set; }
    public required string Keyword { get; set; }

    public List<JobKeywordModel> JobKeywords { get; set; } = [];
}
