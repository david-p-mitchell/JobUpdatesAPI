using JobUpdatesAPI.Interfaces;
using JobUpdatesAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
}
