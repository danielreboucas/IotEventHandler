using IotEventHandler.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iot_event_handler_application.DTO.Request.Devices;
using IotEventHandler.Domain.Entities.Devices;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public IActionResult CreateDevice(CreateDeviceDTO createDeviceDTO)
        {

            var deviceEntity = new DevicesEntity() 
            { 
                IntegrationId = createDeviceDTO.IntegrationId,
                Location = createDeviceDTO.Location, 
                Name = createDeviceDTO.Name
            };

            dbContext.Devices.Add(deviceEntity);
            dbContext.SaveChanges();

            return StatusCode(201, deviceEntity);
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
