// --------------------------------------------------------------------------
// <copyright file="TasslehoffConsumerConfig.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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
    /// Tasslehoff Consumer configuration
    /// </summary>
    [DataContract]
    public class TasslehoffConsumerConfig : Config.Config
    {
        /// <summary>
        /// Gets or sets the backbone endpoint.
        /// </summary>
        /// <value>
        /// The backbone endpoint.
        /// </value>
        [DataMember]
        [ConfigEntry(DefaultValue = "http://localhost/DynNet/Backbone")]
        public string BackboneEndpoint { get; set; }

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
