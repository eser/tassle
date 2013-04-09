//
//  Instance.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Tasslehoff.Library.DataAccess;
using System.Data;
using System.Data.Common;
using Tasslehoff.Library.Config;
using Tasslehoff.Globals;

namespace Tasslehoff.Runner
{
	public class Instance
	{
        public readonly InstanceConfig configuration;

        public Instance(InstanceConfig configuration)
		{
            this.configuration = configuration;

            Database _database = new Database(this.configuration.DatabaseDriver, this.configuration.ConnectionString);
            _database.ExecuteReader(
                "SELECT * FROM users",
                CommandType.Text,
                CommandBehavior.Default,
                null,
                (DbDataReader reader) => {
                    while(reader.Read()) {
                        if(reader["username"] == System.DBNull.Value) {
                            continue;
                        }

                        Console.WriteLine((string)reader["username"]);
                    }
                }
            );
        }
	}
}

