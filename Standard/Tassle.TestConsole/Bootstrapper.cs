using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tassle.Tasks;
using Tassle.Telnet;

namespace Tassle.TestConsole
{
    public class Bootstrapper
    {
        public IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<TelnetServerInterface>(this.CreateTelnetServer());
            services.AddSingleton<TaskManager>();

            return services.BuildServiceProvider();
        }

        public TelnetServerInterface CreateTelnetServer()
        {
            var telnetServer = new TelnetServer(new IPEndPoint(IPAddress.Any, 8084));

            return telnetServer;
        }
    }
}
