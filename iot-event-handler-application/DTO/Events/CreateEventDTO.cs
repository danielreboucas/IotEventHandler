using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_event_handler_application.DTO.Events
{
    public class CreateEventDTO
    {
        public Guid IntegrationId { get; set; }
        public string? Temperature { get; set; }
        public string? Humidity { get; set; }
    }
}
