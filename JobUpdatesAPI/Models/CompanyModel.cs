using System.ComponentModel.DataAnnotations;

namespace JobUpdatesAPI.Models
{
    public class CompanyModel
    {

        [Key]
        public short CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation property to the Jobs
        public ICollection<JobModel> Jobs { get; set; } = [];

        public JobModel? Job { get; set; } = null!; // back to parent
                                                                
        public CompanyModel? Agency { get; set; } = null!; // back to parent
        public override string ToString()
        {
            return $"{CompanyId}: {Name}";
        }
    }
}
