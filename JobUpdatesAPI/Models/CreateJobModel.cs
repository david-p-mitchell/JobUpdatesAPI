namespace JobUpdatesAPI.Models
{
    public class CreateJobModel
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public int MinSalaryExpectation { get; internal set; }
        public int MaxSalaryExpectation { get; internal set; }
    }
}
