using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobUpdatesAPI.Data.Models;

public class JobUpdateModel
{
    [Key]
    public int JobUpdateId { get; set; }
    public int JobId { get; set; }
    public short JobStatusId { get; set; }
    public JobStatusModel? Status { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    // Navigation property to the Job
    [JsonIgnore]  // 🚀 prevents loop
    public JobModel? Job { get; set;  }
}
