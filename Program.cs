using System;
using Microphone;
using Microphone.Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Owin.Hosting;

namespace MicroservicesExample
{
    class Program
    {

        private static int timeout = 10000;
        private static System.Timers.Timer _timer;
        private static string _baseAddress;
        private static string consulHost;

        private static void Main(string[] args)
        {
            consulHost = "localhost";
            if (args.Length > 0)
            {
                consulHost = args[0];
            }

            var port = "4001";
            if (args.Length > 1)
            {
                port = args[1];
            }
            _baseAddress = $"http://localhost:{port}/";
            Console.WriteLine(_baseAddress);
            try
            {
                RegisterService();
                RegisterServerHealthMonitor();
                RunOwinHost();
            }
            finally
            {
                UnregisterService();
            }
        }

        private static void UnregisterService()
        {

        }

        private static void RegisterService()
        {
            try
            {
                Console.WriteLine("Register Service");
                var options = new ConsulOptions
                {
                    Host = consulHost
                    // Port = 8500
                    
                };
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger("logger");
                var provider = new ConsulProvider(loggerFactory, Options.Create(options));
                Console.WriteLine($"Connecting to consul server at: {options.Host}:{options.Port}");
                Cluster.RegisterService(new Uri(_baseAddress), provider, "orders", "v1", logger, new string[] { "v1" });

                Cluster.Client.KeyValuePut("gary", "foo");
                Console.WriteLine("Success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void RunOwinHost()
        {

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(_baseAddress);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", _baseAddress);
            // var instances = Cluster.Client.GetServiceInstances("orders");
            // var instance = instances.FirstOrDefault();

            Console.ReadLine();
            // _timer.Enabled = false;
        }

        private static void RegisterServerHealthMonitor()
        {
            _timer = new System.Timers.Timer { Interval = timeout };
            _timer.Elapsed += ServerHealthCheck;
            _timer.Enabled = true;
            Console.WriteLine("Register Server Health Monitor");
        }

        // When the consul server checks the health of this service we store the last date/time.  If that last check exceeds our threshold we
        // assume that the server has failed and should try to re-register our service!
        private static void ServerHealthCheck(object sender, object args)
        {
            var lastHealthCheck = Startup.LastHealthCheck;
            var diff = DateTime.UtcNow.Subtract(lastHealthCheck).TotalMilliseconds;
            if (!(diff > timeout)) return;
            Console.WriteLine("Server Health Check: Server timeout!!!");
            RegisterService();
        }

    }

}
