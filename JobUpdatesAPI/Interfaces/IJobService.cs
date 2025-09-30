using JobUpdatesAPI.Data.Models;
using JobUpdatesAPI.Models;

namespace JobUpdatesAPI.Interfaces
{
    public interface IJobService
    {
        Task<JobModel> AddJobAsync(AddNewJobModel job);
        Task<IEnumerable<JobModel>> GetAllJobsAsync();
        Task<IEnumerable<JobModel>> GetJobsAsync(int pageNumber = 1, int pageSize = 10);
    }
}
