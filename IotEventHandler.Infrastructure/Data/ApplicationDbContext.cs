using IotEventHandler.Domain.Entities.Devices;
using IotEventHandler.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace IotEventHandler.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<DevicesEntity> Devices { get; set; }
        public DbSet<EventsEntity> Events { get; set; }
    }
}
