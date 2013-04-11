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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Tasslehoff.Globals;
    using Tasslehoff.Library.Config;
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
        /// <exception cref="System.ArgumentException">
        /// Occurs when one of the command line argument has problems.
        /// </exception>
        public static void Main(string[] args)
        {
            Instance instance = null;

            try
            {
                instance = Instance.Create(InstanceOptions.FromCommandLine(args), Console.Out);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (instance == null)
            {
                return;
            }

            CheckSourcesTask checkSourcesTask = new CheckSourcesTask();
            CronItem checkSourcesCronItem = new CronItem(Recurrence.Periodically(TimeSpan.FromSeconds(25)), checkSourcesTask.Do);
            instance.CronManager.Add("checkSources", checkSourcesCronItem);

            FetchStoriesTask fetchStoriesTask = new FetchStoriesTask();
            CronItem fetchStoriesCronItem = new CronItem(Recurrence.Periodically(TimeSpan.FromSeconds(5)), fetchStoriesTask.Do, TimeSpan.FromSeconds(4));
            instance.CronManager.Add("fetchStories", fetchStoriesCronItem);

            ////TestTask testTask = new TestTask();
            ////CronItem testCronItem = new CronItem(Recurrence.Periodically(TimeSpan.FromSeconds(10)), testTask.Do, TimeSpan.FromSeconds(5));
            ////instance.CronManager.Add("test", testCronItem);

            instance.Start();

            Console.ReadLine();

            instance.Stop();
            instance.Dispose();
        }
    }
}
