namespace JobUpdatesAPI.Models;

public class JobStatusModel
{
    public short JobStatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;

    // Navigation property to the JobUpdates
    public ICollection<JobUpdateModel> JobUpdates { get; set; } = [];
    
    public JobUpdateModel? JobUpdate { get; set; } = null!; // back to parent
    // Override ToString for better debugging
    public override string ToString()
    {
        return $"{JobStatusId}: {StatusName}";
    }
}
