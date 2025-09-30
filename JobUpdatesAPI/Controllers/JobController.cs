using JobUpdatesAPI.Data;
using JobUpdatesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobUpdatesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JobController : ControllerBase
{
    private readonly ILogger<JobController> _logger;
    private JobUpdatesDbContext _jobUpdatesDbContext;

    public JobController(ILogger<JobController> logger, JobUpdatesDbContext jobUpdatesDbContext)
    {
        _logger = logger;
        _jobUpdatesDbContext = jobUpdatesDbContext;
    }

    [HttpGet(Name = "GetJobs")]
    public IActionResult Get() => Ok(_jobUpdatesDbContext.Jobs.ToList());

    [HttpGet("JobsWithUpdates", Name = "GetJobsWithUpdates")]
    public IActionResult GetWithUpdates()
    {
        var jobsWithUpdates = _jobUpdatesDbContext.Jobs.Include(job => job.JobUpdates).ToList();
        return Ok(jobsWithUpdates);
    }

    [HttpPost("Create", Name = "CreateJob")]
    public IActionResult CreateJob([FromBody] JobModel job)
    {
        if (job == null) return BadRequest("Job cannot be null");
        
        _jobUpdatesDbContext.Jobs.Add(job);
        _jobUpdatesDbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = job.JobId }, job);
    }

    [HttpPost("Apply", Name = "ApplyJob")]
    public async Task<IActionResult> ApplyJob([FromBody] JobModel job)
    {
        if (job == null) return BadRequest("Job cannot be null");

        var appliedStatus = new JobStatusModel { JobStatusId = 2 };
        _jobUpdatesDbContext.Attach(appliedStatus);

        var appliedUpdate = new JobUpdateModel
        {
            Job = job,
            Status = appliedStatus,
            UpdateDate = DateTime.UtcNow
        };

        job.JobUpdates.Add(appliedUpdate);
        await _jobUpdatesDbContext.Jobs.AddAsync(job);
        await _jobUpdatesDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = job.JobId }, job);
    }
}
