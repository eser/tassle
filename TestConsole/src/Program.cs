// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Tassle.TestConsole {
    public class Program {
        public static Task<int> Main(string[] args) {
            var host = new HostBuilder()
                .UseTassleStartup<Startup>(args)
                .Build();

            var result = host.LogAndRunAsync<Program>();

            System.Console.WriteLine("test");

            return result;
            //TassleCoreConfig tassleConfig = new TassleCoreConfig() {
            //    Culture = "en-us"
            //};

            //TassleCore tassle = new TassleCore(tassleConfig);


            //tassle.Start();

            /*
            DynamicAssembly da = new DynamicAssembly("Deneme");

            var dc = da.AddClass("Eser");

            var df = dc.AddField("a", typeof(int), FieldAttributes.Private);
            var dp = df.ConvertToProperty(dc, "A");

            dc.Finalize();

            da.Save();

            Console.WriteLine("done");
            */

            // initialize bootstrapper
            // var bootstrapper = new Bootstrapper();
            // var serviceProvider = bootstrapper.GetServiceProvider();

            // // start telnet
            // var telnetServer = serviceProvider.GetService<ITelnetServer>();
            // telnetServer.Start();

            // // setup console logging
            // var loggingBuilder = serviceProvider.GetService<ILoggingBuilder>();
            // loggingBuilder
            //     .AddConsole()
            //     .AddDebug()
            //     .AddTelnet(serviceProvider);

            // var fm = new FunctionManager(serviceProvider);
            // fm.RunAsync(
            //     new TestFunction(),
            //     new Request<int>() { Value = 5 });
        }
    }
}
