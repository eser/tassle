// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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

using System;
using Tasslehoff.Tasks;

namespace Tasslehoff.CommandLine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TasslehoffCoreConfig tasslehoffConfig = new TasslehoffCoreConfig()
            {
                Culture = "en-us"
            };

            TasslehoffCore tasslehoff = new TasslehoffCore(tasslehoffConfig);

            var taskItem = new TaskItem(
                    (TaskActionParameters param) =>
                    {
                        Console.WriteLine("helo");
                    }
                )
                .SetRecurrence(Recurrence.Once)
                .SetRepeat(4)
                .Postpone(TimeSpan.FromSeconds(5));


            tasslehoff.AddTask(taskItem);

            tasslehoff.Start();

            /*
            DynamicAssembly da = new DynamicAssembly("Deneme");
            
            var dc = da.AddClass("Eser");

            var df = dc.AddField("a", typeof(int), FieldAttributes.Private);
            var dp = df.ConvertToProperty(dc, "A");
            
            dc.Finalize();

            da.Save();

            Console.WriteLine("done");
            */

            Console.Read();
        }
    }
}
