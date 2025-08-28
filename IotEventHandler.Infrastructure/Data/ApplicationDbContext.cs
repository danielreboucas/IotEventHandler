using IotEventHandler.Domain.Entities.Devices;
using Microsoft.EntityFrameworkCore;

namespace IotEventHandler.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<DevicesEntity> Devices { get; set; }
    }
}
