using System.ComponentModel.DataAnnotations;

namespace IotEventHandler.Domain.Entities.Devices
{
    public class DevicesEntity
    {
        [Key]
        public Guid Uuid { get; set; }
        public string? Name { get; set; }
        public string? IntegrationId { get; set; }
        public string? Location { get; set; }
    }
}
