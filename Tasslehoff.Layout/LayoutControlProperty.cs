// --------------------------------------------------------------------------
// <copyright file="LayoutControlProperty.cs" company="-">
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
using System.Runtime.Serialization;

namespace Tasslehoff.Layout
{
    /// <summary>
    /// LayoutControlProperty class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class LayoutControlProperty
    {
        // fields

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        private string name;

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        private string description;

        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "Value")]
        private string value;
        
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControlProperty"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="value">Value</param>
        public LayoutControlProperty(string name, string description, string value = null)
        {
            this.name = name;
            this.description = description;
            this.value = value;
        }

        // properties

        /// <summary>
        /// Gets or sets name
        /// </summary>
        /// <value>
        /// Name
        /// </value>
        [IgnoreDataMember]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets parent description
        /// </summary>
        /// <value>
        /// Description
        /// </value>
        [IgnoreDataMember]
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets sort index
        /// </summary>
        /// <value>
        /// Sort index
        /// </value>
        [IgnoreDataMember]
        public virtual string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
