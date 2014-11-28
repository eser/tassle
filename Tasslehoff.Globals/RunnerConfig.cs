// -----------------------------------------------------------------------
// <copyright file="RunnerConfig.cs" company="-">
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

namespace Tasslehoff.Globals
{
    using System.Runtime.Serialization;
    using Tasslehoff.Library.Config;

    /// <summary>
    /// Runner configuration
    /// </summary>
    [DataContract]
    public class RunnerConfig : Config
    {
        /// <summary>
        /// Gets or sets the database driver.
        /// </summary>
        /// <value>
        /// The database driver.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "Npgsql")]
        public string DatabaseDriver { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "Server=debian;Port=5432;Database=interesd;User id=interesd;Password=;Pooling=true;Sslmode=Disable")]
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ address.
        /// </summary>
        /// <value>
        /// The RabbitMQ address.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "localhost")]
        public string RabbitMQAddress { get; set; }

        /// <summary>
        /// Gets or sets the memcached addresses.
        /// </summary>
        /// <value>
        /// The memcached addresses.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = null)]
        public string MemcachedAddresses { get; set; }

        /// <summary>
        /// Gets or sets the web service endpoint.
        /// </summary>
        /// <value>
        /// The web service endpoint.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "http://localhost:3535")]
        public string WebServiceEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "en-us")]
        public string Culture { get; set; }
    }
}
