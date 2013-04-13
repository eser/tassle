// -----------------------------------------------------------------------
// <copyright file="TestTask.cs" company="-">
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
    using System.Threading;
    using Tasslehoff.Library.Cron;
    using Tasslehoff.Runner;

    /// <summary>
    /// TestTask class.
    /// </summary>
    public class TestTask : ITask
    {
        /// <summary>
        /// Does the task.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Do(CronActionParameters parameters)
        {
            Instance instance = Instance.Context;

            Console.WriteLine("Started: Test");
            while (!parameters.CancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("Finished: Test");
        }
    }
}
