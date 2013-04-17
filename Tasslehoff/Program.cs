// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
// Copyright (c) 2013 larukedi (eser@sent.com). All rights reserved.
// </copyright>
// <author>larukedi (http://github.com/larukedi/)</author>
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

namespace Tasslehoff
{
    using System;
    using Tasslehoff.Globals;
    using Tasslehoff.Library.Cron;
    using Tasslehoff.Runner;

    /// <summary>
    /// Entry point class of the project.
    /// </summary>
    public class Program
    {
        // methods

        /// <summary>
        /// Entry point method of the project.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Program.ConsoleStart(args);
                return;
            }

            // TODO: Windows Service
            Program.ConsoleStart(args);
        }

        /// <summary>
        /// Consoles the start.
        /// </summary>
        /// <param name="args">The args</param>
        public static void ConsoleStart(string[] args)
        {
            TasslehoffRunner runner = null;

            try
            {
                runner = TasslehoffRunner.Create(RunnerOptions.FromCommandLine(args), Console.Out);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (runner == null)
            {
                return;
            }

            #if DEBUG
            TestTask testTask = new TestTask();
            CronItem testCronItem = new CronItem(Recurrence.Periodically(TimeSpan.FromSeconds(10)), testTask.Do, TimeSpan.FromSeconds(5));
            runner.CronManager.Add("test", testCronItem);
            #endif

            runner.Start();

            Console.ReadLine();

            runner.Stop();
            runner.Dispose();
        }
    }
}
