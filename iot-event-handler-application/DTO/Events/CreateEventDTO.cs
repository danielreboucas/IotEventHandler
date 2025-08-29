using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace iot_event_handler_application.DTO.Events
{
    public class CreateEventDTO
    {
        [JsonPropertyName("deviceId")]
        public Guid IntegrationId { get; set; }
        
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        
        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
        
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [JsonPropertyName("isAlarm")]
        public bool IsAlarm { get; set; }
    }
}
