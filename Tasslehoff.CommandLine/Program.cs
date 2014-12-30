// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
// Copyright (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
// -----------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Tasslehoff.CommandLine
{
    using System;
    using Library.Tasks;
    using Library.Dynamic;
    using System.Reflection;

    public class Program
    {
        public static void Main(string[] args)
        {
            TasslehoffConfig tasslehoffConfig = new TasslehoffConfig()
            {
                Culture = "en-us",
                VerboseMode = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            Tasslehoff tasslehoff = new Tasslehoff(tasslehoffConfig, Console.Out);
            // tasslehoff.QueueManager = new RabbitMQConnection(configuration.RabbitMQAddress);
            // tasslehoff.CacheManager = new MemcachedConnection(configuration.MemcachedAddresses.Split(','));

            tasslehoff.Start();

            TaskItem taskItem = new TaskItem(
                // Recurrence.Once(),
                // Recurrence.OnceAt(DateTimeOffset.UtcNow.AddSeconds(5)),
                Recurrence.Periodically(TimeSpan.FromSeconds(3)),
                (TaskActionParameters param) =>
                {
                    Console.WriteLine("helo");
                }
            );
            tasslehoff.AddTask(taskItem);

            DynamicAssembly da = new DynamicAssembly("Deneme");
            
            var dc = da.AddClass("Eser");

            var df = dc.AddField("a", typeof(int), FieldAttributes.Private);
            var dp = df.ConvertToProperty(dc, "A");
            
            dc.Finalize();

            da.Save();

            Console.WriteLine("done");
            

            Console.Read();
        }
    }
}
