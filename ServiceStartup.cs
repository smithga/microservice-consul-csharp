using Microphone;
using Microphone.Consul;
using MicroservicesExample;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;

namespace MicroServiceExample
{
    public class ServiceStartup
    {

        private static int timeout = 10000;
        private static System.Timers.Timer _timer;
        private static string _baseAddress;

        public ServiceStartup()
        {
            _baseAddress = ConfigurationManager.AppSettings["baseUrl"];
            RegisterService();
            RegisterServerHealthMonitor();
            RunOwinHost();
        }

        public void Start() {
            RegisterService();
        }

        public void Stop() {
            UnregisterService();
        }

        private void UnregisterService()
        {
            
        }

        private void RegisterService()
        {
            try
            {
                Console.WriteLine("Register Service");
                var options = new ConsulOptions
                {
                    Host = ConfigurationManager.AppSettings["consulHost"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["consulPort"])

                };
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger("logger");
                var provider = new ConsulProvider(loggerFactory, Options.Create(options));
                Console.WriteLine($"Connecting to consul server at: {options.Host}:{options.Port}");
                Cluster.RegisterService(new Uri(_baseAddress), provider, "orders", "v1", logger, new string[] { "v1" });
                AddConfiguration();
                Console.WriteLine("Success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void AddConfiguration()
        {
            Cluster.Client.KeyValuePut("foo", "bar");
        }

        private void RunOwinHost()
        {

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(_baseAddress);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", _baseAddress);
        }

        private void RegisterServerHealthMonitor()
        {
            _timer = new System.Timers.Timer { Interval = timeout };
            _timer.Elapsed += ServerHealthCheck;
            _timer.Enabled = true;
            Console.WriteLine("Register Server Health Monitor");
        }

        // When the consul server checks the health of this service we store the last date/time.  If that last check exceeds our threshold we
        // assume that the server has failed and should try to re-register our service!
        private void ServerHealthCheck(object sender, object args)
        {
            var lastHealthCheck = Startup.LastHealthCheck;
            var diff = DateTime.UtcNow.Subtract(lastHealthCheck).TotalMilliseconds;
            if (!(diff > timeout)) return;
            Console.WriteLine("Server Health Check: Server timeout!");
            RegisterService();
        }

    }
}
