using JobUpdatesAPI.Data;
using JobUpdatesAPI.Data.Models;
using JobUpdatesAPI.Models;
using JobUpdatesAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobUpdatesAPI.Tests.ServiceTests;

public class JobServiceTests
{

    private JobUpdatesDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<JobUpdatesDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var dbContext = new JobUpdatesDbContext(options);

        // This applies HasData seeds to the InMemory database
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    private static JobService GetJobService(JobUpdatesDbContext dbContext) => new JobService(dbContext, NullLogger<JobService>.Instance);

    [Fact]
    public async Task AddJobAsync_Should_Add_Job_To_Database()
    {
        // Arrange
        var dbContext = GetDbContext(nameof(AddJobAsync_Should_Add_Job_To_Database));
        var service = GetJobService(dbContext);

        var newJob = new AddNewJobModel
        {
            Name = "Software Engineer",
            Description = "Backend development",
            MinSalaryExpectation = 50000,
            MaxSalaryExpectation = 70000,
            HaveApplied = false
        };

        // Act
        var result = await service.AddJobAsync(newJob);

        // Assert
        Assert.True(result.JobId > 0);
        Assert.Single(dbContext.Jobs);
        Assert.Equal("Software Engineer", dbContext.Jobs.First().Name);
        Assert.Empty(dbContext.JobUpdates);
    }

    [Fact]
    public async Task AddJobAsync_Should_Create_JobUpdate_When_HaveApplied_Is_True()
    {
        // Arrange
        var dbContext = GetDbContext(nameof(AddJobAsync_Should_Create_JobUpdate_When_HaveApplied_Is_True));
        var service = GetJobService(dbContext);

        var newJob = new AddNewJobModel
        {
            Name = "Frontend Developer",
            Description = "React and TypeScript",
            MinSalaryExpectation = 40000,
            MaxSalaryExpectation = 60000,
            HaveApplied = true
        };

        // Act
        var result = await service.AddJobAsync(newJob);

        // Assert
        Assert.Single(dbContext.Jobs);
        Assert.Equal("Frontend Developer", dbContext.Jobs.First().Name);
        var jobStatuses = dbContext.JobStatuses;
        var jobUpdate = dbContext.JobUpdates.FirstOrDefault();
        Assert.NotNull(jobUpdate);
        Assert.Equal(result.JobId, jobUpdate.Job!.JobId);
        Assert.Equal(2, jobUpdate.JobStatusId);
    }

    [Fact]
    public async Task GetJobsAsync_Should_Return_All_Jobs()
    {
        // Arrange
        var dbContext = GetDbContext(nameof(GetJobsAsync_Should_Return_All_Jobs));
        dbContext.Jobs.AddRange(
            new JobModel { Name = "QA Tester", Description = "Testing software" },
            new JobModel { Name = "DevOps Engineer", Description = "CI/CD pipelines" }
        );
        await dbContext.SaveChangesAsync();

        var service = GetJobService(dbContext);

        // Act
        var jobs = await service.GetJobsAsync();

        // Assert
        Assert.Equal(2, jobs.Count());
        Assert.Contains(jobs, j => j.Name == "QA Tester");
        Assert.Contains(jobs, j => j.Name == "DevOps Engineer");
    }
}