// -----------------------------------------------------------------------
// <copyright file="CheckSourcesTask.cs" company="-">
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

namespace Tasslehoff.Runner.Tasks
{
    using System;
    using System.Data;
    using System.Data.Common;
    using Tasslehoff.Globals.Entities;
    using Tasslehoff.Library.Cron;
    using Tasslehoff.Library.DataAccess;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// CheckSourcesTask class.
    /// </summary>
    public class CheckSourcesTask : ITask
    {
        /// <summary>
        /// Does the task.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Do(CronActionParameters parameters)
        {
            Instance instance = Instance.Context;
            DataEntity<User> users = new DataEntity<User>();

            Console.WriteLine("Started: CheckSources");
            instance.Database.ExecuteReader(
                "SELECT * FROM users",
                CommandType.Text,
                CommandBehavior.Default,
                null,
                (DbDataReader reader) =>
                {
                    while (reader.Read())
                    {
                        User user = users.GetItem(reader);
                        if (user.Username == null)
                        {
                            continue;
                        }

                        byte[] serializedData = VariableUtils.Serialize(user);
                        Instance.Context.MessageQueue.Enqueue("task_queue", serializedData);
                    }
                });

            //// var client = new FacebookClient();
            //// dynamic me = client.Get("larukedi");
            //// Console.WriteLine(me.name);
            Console.WriteLine("Finished: CheckSources");
        }
    }
}
