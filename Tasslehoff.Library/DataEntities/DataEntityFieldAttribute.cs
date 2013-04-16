// -----------------------------------------------------------------------
// <copyright file="DataEntityFieldAttribute.cs" company="-">
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
    using System.Reflection;

    /// <summary>
    /// An attribute class for DataEntity fields.
    /// </summary>
    public class DataEntityFieldAttribute : Attribute
    {
        // fields

        /// <summary>
        /// The class member
        /// </summary>
        private MemberInfo classMember;

        /// <summary>
        /// The field name
        /// </summary>
        private string fieldName;

        /// <summary>
        /// The type
        /// </summary>
        private Type type;

        /// <summary>
        /// The serializer
        /// </summary>
        private string serializer = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityFieldAttribute"/> class.
        /// </summary>
        public DataEntityFieldAttribute() : base()
        {
        }

        // properties

        /// <summary>
        /// Gets or sets the class member.
        /// </summary>
        /// <value>
        /// The class member.
        /// </value>
        public MemberInfo ClassMember
        {
            get
            {
                return this.classMember;
            }

            set
            {
                this.classMember = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }

            set
            {
                this.fieldName = value;
            }
        }

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
        /// Gets or sets the serializer.
        /// </summary>
        /// <value>
        /// The serializer.
        /// </value>
        public string Serializer
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
    }
}
