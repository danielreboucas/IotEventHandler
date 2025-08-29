using iot_event_handler_application.DTO.Devices;
using IotEventHandler.Domain.Entities.Devices;
using IotEventHandler.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace iot_event_handler.Controllers.Devices
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public DevicesController(
            ILogger<DevicesController> logger, 
            ApplicationDbContext applicationDbContext, 
            HttpClient httpClient, 
            IConfiguration configuration)
        {
            _logger = logger;
            dbContext = applicationDbContext;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice(CreateDeviceDTO createDeviceDTO)
        {

            var deviceEntity = new DevicesEntity()
            {
                Location = createDeviceDTO.Location,
                Name = createDeviceDTO.Name
            };

            var callbackUrl = _configuration["ExternalApiSettings:CallbackUrl"];
            var externalApiUrl = _configuration["ExternalApiSettings:BaseUrl"];

            var response = await _httpClient.PostAsJsonAsync($"{externalApiUrl}/register", new
            {
                deviceName = deviceEntity.Name,
                location = deviceEntity.Location,
                callbackUrl = $"{callbackUrl}/events"

            });

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<dynamic>(responseContent);
                deviceEntity.IntegrationId = responseData.GetProperty("integrationId").GetString();

                dbContext.Devices.Add(deviceEntity);
                dbContext.SaveChanges();

                return StatusCode(201, deviceEntity);
            }
    
            _logger.LogError($"External API call failed with status: {response.StatusCode}");
            return StatusCode(424, "Failed to generate an integrationId.");
        }

        [HttpGet]
        public IActionResult GetAllDevices()
        {
            var devices = dbContext.Devices.ToList();
            return Ok(devices);
        }

        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> GetDeviceByUUID(Guid uuid)
        {
            var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.Uuid == uuid);

            if (device == null)
            {
                _logger.LogError("Error occurred while retrieving devices");
                return NotFound();
            }

            return Ok(device);
        }

        [HttpPut("{uuid:guid}")]
        public string UpdateDevice()
        {
            string test = "opa";
            return test;
        }

        [HttpDelete("{uuid:guid}")]
        public async Task<IActionResult> DeleteDevice(Guid uuid)
        {
            var device = dbContext.Devices.Find(uuid);

            if (device == null)
            {
                return NotFound();
            }

            var externalApiUrl = _configuration["ExternalApiSettings:BaseUrl"];
            var response = await _httpClient.DeleteAsync($"{externalApiUrl}/unregister/{device.IntegrationId}");

            if (response.IsSuccessStatusCode)
            {
                dbContext.Devices.Remove(device);
                dbContext.SaveChanges();

                return Ok("Device deleted successfully!");
            }

            _logger.LogError($"External API call failed with status: {response.StatusCode}");
            return StatusCode(500, "Failed to delete the device.");
        }
    }
}
