// --------------------------------------------------------------------------
// <copyright file="TasslehoffCoreConfig.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

using System.Runtime.Serialization;
using Tasslehoff.Config;

namespace Tasslehoff
{
    /// <summary>
    /// Tasslehoff Core configuration
    /// </summary>
    [DataContract]
    public class TasslehoffCoreConfig : Config.Config
    {
        /// <summary>
        /// Gets or sets the database provider name.
        /// </summary>
        /// <value>
        /// The database provider name.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "Npgsql")]
        public string DatabaseProviderName { get; set; }

        /// <summary>
        /// Gets or sets the database provider manifest token.
        /// </summary>
        /// <value>
        /// The database provider manifest token.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string DatabaseProviderManifestToken { get; set; }

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
        /// Gets or sets the rabbitmq address.
        /// </summary>
        /// <value>
        /// The rabbitmq address.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string RabbitMQAddress { get; set; }

        /// <summary>
        /// Gets or sets the ironmq projectid.
        /// </summary>
        /// <value>
        /// The ironmq projectid.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string IronMQProjectId { get; set; }

        /// <summary>
        /// Gets or sets the ironmq token.
        /// </summary>
        /// <value>
        /// The ironmq token.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string IronMQToken { get; set; }

        /// <summary>
        /// Gets or sets the memcached address.
        /// </summary>
        /// <value>
        /// The memcached address.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string MemcachedAddress { get; set; }

        /// <summary>
        /// Gets or sets the redis address.
        /// </summary>
        /// <value>
        /// The redis address.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string RedisAddress { get; set; }

        /// <summary>
        /// Gets or sets the elastic search address.
        /// </summary>
        /// <value>
        /// The elastic search address.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "")]
        public string ElasticSearchAddress { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "en-us")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the extension paths.
        /// </summary>
        /// <value>
        /// The extension paths.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = null)]
        public string[] ExtensionPaths { get; set; }
    }
}
