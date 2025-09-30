using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobUpdatesAPI.Data.Models;

public class JobModel
{
    [Key]
    public int JobId { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public int MinSalaryExpectation { get; set; }
    public int MaxSalaryExpectation { get; set; }
    public int StatedSalaryExpectation { get; set; }

    public JobUpdateModel? LastUpdate => JobUpdates.OrderByDescending(update => update.UpdateDate).FirstOrDefault();
    
    [JsonIgnore] 
    public ICollection<JobUpdateModel> JobUpdates { get; set; } = [];

    public List<JobKeywordModel> JobKeywords { get; set; } = [];

}
