using IotEventHandler.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iot_event_handler.Controllers.Devices
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly ApplicationDbContext dbContext;
        
        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            dbContext = applicationDbContext;
        }

        [HttpPost]
        public string Create()
        {
            string test = "opa";
            return test;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var devices = dbContext.Devices.ToList();
            return Ok(devices);
        }

        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid)
        {
            var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.Uuid == uuid);

            _logger.LogInformation("device", device);
            if (device == null)
            {
                _logger.LogError("Error occurred while retrieving devices");
                return NotFound();
            }

            return Ok(device);
        }

    //[HttpDelete("devices")]
    //public string Store()
    //{
    //    string test = "opa";
    //    return test;
    //}
}
}
