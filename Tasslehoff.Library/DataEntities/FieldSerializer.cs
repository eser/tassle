// -----------------------------------------------------------------------
// <copyright file="FieldSerializer.cs" company="-">
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

namespace Tasslehoff.Library.DataEntities
{
    using System;

    /// <summary>
    /// FieldSerializer class.
    /// </summary>
    public class FieldSerializer
    {
        // fields

        /// <summary>
        /// The serializer
        /// </summary>
        private Func<object, object> serializer;

        /// <summary>
        /// The deserializer
        /// </summary>
        private Func<object, object> deserializer;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSerializer" /> class.
        /// </summary>
        /// <param name="deserializer">The deserializer</param>
        /// <param name="serializer">The serializer</param>
        public FieldSerializer(Func<object, object> deserializer = null, Func<object, object> serializer = null)
        {
            this.deserializer = deserializer;
            this.serializer = serializer;

            if (this.serializer == null)
            {
                this.serializer = (object value) =>
                {
                    if (value == null)
                    {
                        return null;
                    }

                    return value.ToString();
                };
            }
        }

        // properties

        /// <summary>
        /// Gets or sets the serializer.
        /// </summary>
        /// <value>
        /// The serializer.
        /// </value>
        public Func<object, object> Serializer
        {
            get
            {
                return this.serializer;
            }

            set
            {
                this.serializer = value;
            }
        }

        /// <summary>
        /// Gets or sets the deserializer.
        /// </summary>
        /// <value>
        /// The deserializer.
        /// </value>
        public Func<object, object> Deserializer
        {
            get
            {
                return this.deserializer;
            }

            set
            {
                this.deserializer = value;
            }
        }
    }
}
