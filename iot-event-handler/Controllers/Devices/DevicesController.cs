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

            try
            {
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
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"External API call failed with status: {response.StatusCode}");
                }
                 
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Failed to call external API");
            }


            dbContext.Devices.Add(deviceEntity);
            dbContext.SaveChanges();

            return StatusCode(201, deviceEntity);
            
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

            _logger.LogInformation("device", device);
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
        public string DeleteDevice()
        {
            string test = "opa";
            return test;
        }
    }
}
