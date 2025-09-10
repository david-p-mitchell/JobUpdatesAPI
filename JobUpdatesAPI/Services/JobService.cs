using JobUpdatesAPI.Data;
using JobUpdatesAPI.Data.Models;
using JobUpdatesAPI.Interfaces;
using JobUpdatesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobUpdatesAPI.Services
{
    public class JobService(JobUpdatesDbContext dbContext, ILogger<JobService> logger) : IJobService
    {
        private readonly JobUpdatesDbContext _dbContext = dbContext;
        private readonly ILogger<JobService> _logger = logger;


        public async Task<JobModel> AddJobAsync(AddNewJobModel addNewJob)
        {
            try
            {
                if (addNewJob == null)
                    throw new ArgumentNullException(nameof(addNewJob));

                var newJob = new JobModel
                {
                    Description = addNewJob.Description,
                    Name = addNewJob.Name,
                    MinSalaryExpectation = addNewJob.MinSalaryExpectation,
                    MaxSalaryExpectation = addNewJob.MaxSalaryExpectation
                };

                _dbContext.Jobs.Add(newJob);

                if (addNewJob.HaveApplied)
                {
                    var appliedJob = new JobUpdateModel
                    {
                        Job = newJob,
                        JobStatusId = 2
                    };

                    _dbContext.JobUpdates.Add(appliedJob);
                }

                await _dbContext.SaveChangesAsync();
                return newJob;
            }
            catch (Exception)
            {
                _logger.LogError("Something went wrong adding a new Job");
                throw;
            }
        }

        public async Task<IEnumerable<JobModel>> GetAllJobsAsync() => await _dbContext.Jobs.ToListAsync();

        public async Task<IEnumerable<JobModel>> GetJobsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return await _dbContext.Jobs
                .OrderBy(j => j.JobId) // ensure consistent ordering
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



    }
}
