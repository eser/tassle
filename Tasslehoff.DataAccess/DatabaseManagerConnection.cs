// --------------------------------------------------------------------------
// <copyright file="DatabaseManagerConnection.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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
using System.Runtime.Serialization;

namespace Tasslehoff.DataAccess
{
    /// <summary>
    /// DataQueryManagerItem class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DatabaseManagerConnection
    {
        // fields

        /// <summary>
        /// Provider name.
        /// </summary>
        [DataMember(Name = "ProviderName")]
        private string providerName;

        /// <summary>
        /// Provider manifest token.
        /// </summary>
        [DataMember(Name = "ProviderManifestToken")]
        private string providerManifestToken;

        /// <summary>
        /// Connection string.
        /// </summary>
        [DataMember(Name = "ConnectionString")]
        private string connectionString;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManagerConnection"/> class.
        /// </summary>
        public DatabaseManagerConnection()
        {
        }

        // properties

        /// <summary>
        /// Gets or Sets the provider name.
        /// </summary>
        [IgnoreDataMember]
        public string ProviderName
        {
            get
            {
                return this.providerName;
            }
            set
            {
                this.providerName = value;
            }
        }

        /// <summary>
        /// Gets or Sets the provider manifest token.
        /// </summary>
        [IgnoreDataMember]
        public string ProviderManifestToken
        {
            get
            {
                return this.providerManifestToken;
            }
            set
            {
                this.providerManifestToken = value;
            }
        }

        /// <summary>
        /// Gets or Sets the connection string.
        /// </summary>
        [IgnoreDataMember]
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }
    }
}
