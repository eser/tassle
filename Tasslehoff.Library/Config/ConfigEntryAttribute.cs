// -----------------------------------------------------------------------
// <copyright file="ConfigEntryAttribute.cs" company="-">
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

namespace Tasslehoff.Library.Config
{
    using System;

    /// <summary>
    /// Attribute for config entries.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigEntryAttribute : Attribute
    {
        // fields

        /// <summary>
        /// The default value
        /// </summary>
        private object defaultValue;

        /// <summary>
        /// Whether is encrypted or not
        /// </summary>
        private bool isEncrypted;

        /// <summary>
        /// Whether will be skipped during on reset
        /// </summary>
        private bool skipInReset;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigEntryAttribute"/> class.
        /// </summary>
        public ConfigEntryAttribute() : base()
        {
        }

        // attributes

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }

            set
            {
                this.defaultValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is encrypted or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if is encrypted; otherwise, <c>false</c>.
        /// </value>
        public bool IsEncrypted
        {
            get
            {
                return this.isEncrypted;
            }

            set
            {
                this.isEncrypted = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether will be skipped on reset.
        /// </summary>
        /// <value>
        ///   <c>true</c> if will be skipped on reset; otherwise, <c>false</c>.
        /// </value>
        public bool SkipInReset
        {
            get
            {
                return this.skipInReset;
            }

            set
            {
                this.skipInReset = value;
            }
        }
    }
}
