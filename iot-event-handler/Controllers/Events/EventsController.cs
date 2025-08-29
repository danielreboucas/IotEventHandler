using iot_event_handler_application.DTO.Events;
using IotEventHandler.Domain.Entities.Events;
using IotEventHandler.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace iot_event_handler.Controllers.Events
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ApplicationDbContext dbContext;

        public EventsController(
            ILogger<EventsController> logger,
            ApplicationDbContext applicationDbContext
        )
        {
            _logger = logger;
            dbContext = applicationDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO createEventDTO)
        {
            var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.IntegrationId == createEventDTO.IntegrationId.ToString());

            if (device == null)
            {
                _logger.LogError("Device with IntegrationId {IntegrationId} not found", createEventDTO.IntegrationId);
                return NotFound($"Device with IntegrationId {createEventDTO.IntegrationId} not found.");
            }

            var eventEntity = new EventsEntity()
            {
                Temperature = createEventDTO.Temperature.ToString("F2"),
                Humidity = createEventDTO.Humidity.ToString("F2"),
                DeviceUuid = device.Uuid
            };

            dbContext.Events.Add(eventEntity);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Event created successfully for device {DeviceUuid}", device.Uuid);
            return StatusCode(201, createEventDTO);
        }
    }
}
