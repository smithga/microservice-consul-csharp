using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microphone;
using Microsoft.Extensions.Logging;

namespace test1
{

    [Route("orders")]
    public class OrdersController : ApiController
    {

        public string Get()
        {
            return "Orders Service";
        }
    }

    [Route("status")]
    public class StatusController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IHealthCheck _healthCheck;

        public StatusController()
        {
        }

        public StatusController(ILoggerFactory loggerFactory, IHealthCheck checkHealth = null)
        {
            _logger = loggerFactory.CreateLogger("Microphone");
            _healthCheck = checkHealth;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            var machine = Environment.MachineName;
            Startup.LastHealthCheck = DateTime.UtcNow;
            if (_logger != null)
            {
                _logger.LogInformation("Status OK");
                if (_healthCheck != null)
                {
                    await _healthCheck.CheckHealth();
                }
            }
            var status = new
            {
                Machine = machine,
                Result = "OK",
            };
            return status;
        }
    }

}
