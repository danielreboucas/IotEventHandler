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
                _logger.LogError($"Device with IntegrationId {createEventDTO.IntegrationId} not found.");
                return NotFound($"Device with IntegrationId {createEventDTO.IntegrationId} not found.");
            }

            var eventEntity = new EventsEntity()
            {
                Temperature = createEventDTO.Temperature,
                Humidity = createEventDTO.Humidity,
                DeviceUuid = device.Uuid
            };

            dbContext.Events.Add(eventEntity);
            dbContext.SaveChanges();

            _logger.LogError($"Event created: {createEventDTO}");
            return StatusCode(201, createEventDTO);
        }
    }
}
