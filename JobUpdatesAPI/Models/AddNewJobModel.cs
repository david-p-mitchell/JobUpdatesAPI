namespace JobUpdatesAPI.Models;

public class AddNewJobModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MinSalaryExpectation { get; set; }
    public int MaxSalaryExpectation { get; set; }
    public bool HaveApplied { get; set; }
}
