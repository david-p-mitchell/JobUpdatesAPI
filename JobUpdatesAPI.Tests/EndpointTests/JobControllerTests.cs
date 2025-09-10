using System.Linq;
using System.Threading.Tasks;
using JobUpdatesAPI.Controllers;
using JobUpdatesAPI.Data;
using JobUpdatesAPI.Data.Models;
using JobUpdatesAPI.Models;
using JobUpdatesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace JobUpdatesAPI.Tests
{
    public class JobControllerTests
    {
        // Setup InMemory DbContext with seeded jobs and JobStatusModel
        private JobUpdatesDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<JobUpdatesDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new JobUpdatesDbContext(options);
            dbContext.Database.EnsureCreated(); // ensures HasData seeds are applied

            // Seed jobs if not already present
            if (!dbContext.Jobs.Any())
            {
                dbContext.Jobs.AddRange(
                    new JobModel { Name = "Job 1", Description = "Desc 1" },
                    new JobModel { Name = "Job 2", Description = "Desc 2" },
                    new JobModel { Name = "Job 3", Description = "Desc 3" },
                    new JobModel { Name = "Job 4", Description = "Desc 4" }
                );
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private JobService GetJobService(JobUpdatesDbContext dbContext) => new (dbContext, NullLogger<JobService>.Instance);

        private JobController GetController(JobService service) => new (service);

        [Fact]
        public async Task GetAll_Should_Return_All_Jobs()
        {
            // Arrange
            var dbContext = GetDbContext(nameof(GetAll_Should_Return_All_Jobs));
            var service = GetJobService(dbContext);
            var controller = GetController(service);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jobs = Assert.IsAssignableFrom<IEnumerable<JobModel>>(okResult.Value);
            Assert.Equal(4, jobs.Count());
        }

        [Fact]
        public async Task Get_Should_Return_Paginated_Jobs()
        {
            // Arrange
            var dbContext = GetDbContext(nameof(Get_Should_Return_Paginated_Jobs));
            var service = GetJobService(dbContext);
            var controller = GetController(service);

            int pageNumber = 2;
            int pageSize = 2;

            // Act
            var result = await controller.Get(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jobs = Assert.IsAssignableFrom<IEnumerable<JobModel>>(okResult.Value);

            Assert.Equal(2, jobs.Count());
            Assert.Contains(jobs, j => j.Name == "Job 3");
            Assert.Contains(jobs, j => j.Name == "Job 4");
        }

        [Fact]
        public async Task AddJob_Should_Add_Job_And_JobUpdate_When_HaveApplied_True()
        {
            // Arrange
            var dbContext = GetDbContext(nameof(AddJob_Should_Add_Job_And_JobUpdate_When_HaveApplied_True));
            var service = GetJobService(dbContext);
            var controller = GetController(service);

            var newJob = new AddNewJobModel
            {
                Name = "New Job",
                Description = "Test Description",
                MinSalaryExpectation = 50000,
                MaxSalaryExpectation = 70000,
                HaveApplied = true // test JobUpdate creation
            };

            // Act
            var result = await controller.AddJob(newJob);

            // Assert
            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnedJob = Assert.IsType<JobModel>(createdResult.Value);
            Assert.Equal("New Job", returnedJob.Name);

            // Verify job exists in DB
            var jobInDb = dbContext.Jobs.FirstOrDefault(j => j.JobId == returnedJob.JobId);
            Assert.NotNull(jobInDb);
            Assert.Equal("New Job", jobInDb.Name);

            // Verify JobUpdate created
            var jobUpdateInDb = dbContext.JobUpdates.FirstOrDefault(ju => ju.JobId == returnedJob.JobId);
            Assert.NotNull(jobUpdateInDb);
            Assert.Equal(2, jobUpdateInDb.JobStatusId); // matches "Applied" seed
        }

        [Fact]
        public async Task AddJob_Should_Add_Job_Without_JobUpdate_When_HaveApplied_False()
        {
            // Arrange
            var dbContext = GetDbContext(nameof(AddJob_Should_Add_Job_Without_JobUpdate_When_HaveApplied_False));
            var service = GetJobService(dbContext);
            var controller = GetController(service);

            var newJob = new AddNewJobModel
            {
                Name = "Job Without Update",
                Description = "No Update",
                MinSalaryExpectation = 40000,
                MaxSalaryExpectation = 60000,
                HaveApplied = false
            };

            // Act
            var result = await controller.AddJob(newJob);

            // Assert
            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnedJob = Assert.IsType<JobModel>(createdResult.Value);
            Assert.Equal("Job Without Update", returnedJob.Name);

            // Verify JobUpdate NOT created
            var jobUpdateInDb = dbContext.JobUpdates.FirstOrDefault(ju => ju.JobId == returnedJob.JobId);
            Assert.Null(jobUpdateInDb);
        }
    }
}
