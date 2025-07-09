namespace JobUpdatesAPI.Models;

public class JobUpdateModel
{
    public int JobUpdateId { get; set; }
    public int JobId { get; set; }
    public short JobStatusId { get; set; }
    public JobStatusModel? Status { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    // Navigation property to the Job
    public JobModel? Job { get; set;  }
}
