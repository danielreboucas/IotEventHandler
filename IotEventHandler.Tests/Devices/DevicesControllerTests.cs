using iot_event_handler.Controllers.Devices;
using iot_event_handler_application.DTO.Devices;
using IotEventHandler.Domain.Entities.Devices;
using IotEventHandler.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace IotEventHandler.Tests.Devices
{
    [TestClass]
    public class DevicesControllerTests
    {

        private static DevicesController CreateController(ApplicationDbContext context)
        {
            var logger = Mock.Of<ILogger<DevicesController>>();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ExternalApiSettings:CallbackUrl"] = "http://callback",
                    ["ExternalApiSettings:BaseUrl"] = "http://external"
                })
                .Build();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage());
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            return new DevicesController(
                logger,
                context,
                httpClient,
                config
            );
        }

        [TestMethod]
        public async Task GetDeviceByUUID_ReturnsDeviceWhenDeviceExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetDeviceByUUID_ReturnsDeviceWhenDeviceExists))
                .Options;

            using var context = new ApplicationDbContext(options);
            var deviceUUID = Guid.NewGuid();
            var device = new DevicesEntity
            {
                Uuid = deviceUUID,
                Name = "Test Name",
                Location = "Test Location",
                IntegrationId = "test-integration-id"
            };

            context.Devices.Add(device);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.GetDeviceByUUID(deviceUUID);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedDevice = okResult.Value as DevicesEntity;
            Assert.IsNotNull(returnedDevice);
            Assert.AreEqual(deviceUUID, returnedDevice.Uuid);
            Assert.AreEqual("Test Name", returnedDevice.Name);
            Assert.AreEqual("Test Location", returnedDevice.Location);
            Assert.AreEqual("test-integration-id", returnedDevice.IntegrationId);
        }

        [TestMethod]
        public async Task GetDeviceByUUID_ReturnsNotFoundWhenDeviceDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetDeviceByUUID_ReturnsNotFoundWhenDeviceDoesNotExist))
                .Options;

            using var context = new ApplicationDbContext(options);
            var controller = CreateController(context);
            var nonExistentUUID = Guid.NewGuid();

            var result = await controller.GetDeviceByUUID(nonExistentUUID);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetAllDevices_ReturnsEmptyListWhenNoDevicesExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetAllDevices_ReturnsEmptyListWhenNoDevicesExist))
                .Options;

            using var context = new ApplicationDbContext(options);
            var controller = CreateController(context);

            var result = controller.GetAllDevices();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var devices = okResult.Value as List<DevicesEntity>;
            Assert.IsNotNull(devices);
            Assert.IsEmpty(devices);
        }

        [TestMethod]
        public async Task GetAllDevices_ReturnsAllDevicesWhenDevicesExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetAllDevices_ReturnsAllDevicesWhenDevicesExist))
                .Options;

            using var context = new ApplicationDbContext(options);
            
            var device1 = new DevicesEntity
            {
                Uuid = Guid.NewGuid(),
                Name = "Device 1",
                Location = "Location 1",
                IntegrationId = "integration-id-1"
            };

            var device2 = new DevicesEntity
            {
                Uuid = Guid.NewGuid(),
                Name = "Device 2",
                Location = "Location 2",
                IntegrationId = "integration-id-2"
            };

            context.Devices.AddRange(device1, device2);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = controller.GetAllDevices();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var devices = okResult.Value as List<DevicesEntity>;
            Assert.IsNotNull(devices);
            Assert.HasCount(2, devices);
            Assert.IsTrue(devices.Any(d => d.Name == "Device 1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Device 2"));
        }
    }
}