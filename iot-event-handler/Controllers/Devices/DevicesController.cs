using Microsoft.AspNetCore.Mvc;

namespace iot_event_handler.Controllers.Devices
{
    [ApiController]
    [Route("api/")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ILogger<DevicesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("devices")]
        public string Store()
        {
            string test = "opa";
            return test;
        }

        [HttpGet("devices")]
        public string Get()
        {
            string test = "opa";
            return test;
        }

    //[HttpDelete("devices")]
    //public string Store()
    //{
    //    string test = "opa";
    //    return test;
    //}
}
}
