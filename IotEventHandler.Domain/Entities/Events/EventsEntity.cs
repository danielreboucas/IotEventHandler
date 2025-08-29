using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotEventHandler.Domain.Entities.Events
{
    public class EventsEntity
    {
        [Key]
        public Guid Uuid { get; set; }
        public Guid DeviceUuid { get; set; }
        public string? Temperature { get; set; }
        public string? Humidity { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAlarm { get; set; }
    }
}
