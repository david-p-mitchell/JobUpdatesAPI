using System.ComponentModel.DataAnnotations;

namespace JobUpdatesAPI.Models;

public class JobModel
{
    [Key]
    public int JobId { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public int MinSalaryExpectation { get; set; }
    public int MaxSalaryExpectation { get; set; }

    public JobUpdateModel? LastUpdate => JobUpdates.OrderByDescending(update => update.UpdateDate).FirstOrDefault();
    public ICollection<JobUpdateModel> JobUpdates { get; set; } = [];
    public List<JobKeywordModel> JobKeywords { get; set; } = [];

}
