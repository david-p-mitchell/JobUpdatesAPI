using JobUpdatesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobUpdatesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController(IJobService jobService) : ControllerBase
{
    private readonly IJobService _jobService = jobService;

    [HttpGet(Name = "GetAllJobs")]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [HttpGet(Name = "GetJobs")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {

        var jobs = await _jobService.GetJobsAsync(pageNumber, pageSize);
        return Ok(jobs);
    }

    [HttpPost(Name = "AddJob")]
    public async Task<IActionResult> AddJob(AddNewJobModel job)
    {
        if (job == null)
            return BadRequest("Job cannot be null.");

        var newJob = await _jobService.AddJobAsync(job);

        return CreatedAtRoute("GetJobs", new { id = newJob.JobId }, newJob);
    }
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
