// --------------------------------------------------------------------------
// <copyright file="LayoutPropertiesAttribute.cs" company="-">
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

using System;

namespace Tasslehoff.Layout.Common
{
    /// <summary>
    /// An attribute class for Layouts.
    /// </summary>
    public class LayoutPropertiesAttribute : Attribute
    {
        // fields

        /// <summary>
        /// The type
        /// </summary>
        private Type type;

        /// <summary>
        /// The display name
        /// </summary>
        private string displayName;

        /// <summary>
        /// The icon
        /// </summary>
        private string icon;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPropertiesAttribute"/> class.
        /// </summary>
        public LayoutPropertiesAttribute()
            : base()
        {
        }

        // properties

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }
    }
}
