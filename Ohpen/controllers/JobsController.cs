using Microsoft.AspNetCore.Mvc;
using Ohpen.services.interfaces;

namespace Ohpen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService jobService;

        public JobsController(IJobService _jobService)
        {
            jobService = _jobService;
        }

        [HttpPost]
        public IActionResult StartJob(string _type, [FromBody] List<object> _data)
        {
            Console.WriteLine(_type);
            var guid = jobService.StartJob(_type, _data);
            if (guid == Guid.Empty)
            {
                return BadRequest();
            }
            return Accepted(guid);
        }

        [HttpGet("{_guid}/status")]
        public IActionResult GetJobStatus(Guid _guid)
        {
            return GetResponse(jobService.GetJobStatus(_guid));
        }

        [HttpGet("{_guid}/logs")]
        public IActionResult GetJobLogs(Guid _guid)
        {
            return GetResponse(jobService.GetJobLogs(_guid));
        }

        private IActionResult GetResponse<T>(T _object)
        {
            if (_object == null)
            {
                return NotFound();
            }
            return Ok(_object);
        }
    }
}
