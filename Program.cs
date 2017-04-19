using Topshelf;
using MicroServiceExample;

namespace MicroservicesExample
{
    class Program
    {

        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceStartup>(s =>
                {
                    s.ConstructUsing(name => new ServiceStartup());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Microservice Example");
                x.SetDisplayName("Microservice Example");
                x.SetServiceName("Microservice Example");
            });
        }

    }

}
