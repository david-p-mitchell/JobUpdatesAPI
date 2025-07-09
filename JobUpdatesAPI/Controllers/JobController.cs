using JobUpdatesAPI.Data;
using Microsoft.AspNetCore.Mvc;

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
}
