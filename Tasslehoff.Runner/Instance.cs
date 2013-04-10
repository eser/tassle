// -----------------------------------------------------------------------
// <copyright file="Instance.cs" company="-">
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

namespace Tasslehoff.Runner
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Tasslehoff.Globals;
    using Tasslehoff.Globals.Entities;
    using Tasslehoff.Library.Cron;
    using Tasslehoff.Library.DataAccess;

    /// <summary>
    /// Instance class.
    /// </summary>
    public class Instance
    {
        // fields

        /// <summary>
        /// The context
        /// </summary>
        private static Instance context = null;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly InstanceConfig configuration;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Instance"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Instance(InstanceConfig configuration)
        {
            // singleton pattern
            if (Instance.context == null)
            {
                Instance.context = this;
            }

            this.configuration = configuration;

            DataEntity<User> users = new DataEntity<User>();

            Database database = new Database(this.configuration.DatabaseDriver, this.configuration.ConnectionString);
            database.ExecuteReader(
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

                        Console.WriteLine((string)reader["username"]);
                    }
                });

            //// var client = new FacebookClient();
            //// dynamic me = client.Get("larukedi");
            //// Console.WriteLine(me.name);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");

            CronManager cronManager = new CronManager();
            cronManager.Add(
                "cron",
                new CronItem(
                    Recurrence.Periodically(TimeSpan.FromSeconds(5)),
                    () =>
                    {
                        Console.WriteLine("test");
                    }));

            cronManager.Start();

            Console.Read();
        }

        // properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public static Instance Context
        {
            get
            {
                return Instance.context;
            }
        }
    }
}
