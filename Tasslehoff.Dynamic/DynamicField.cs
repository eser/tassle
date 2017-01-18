// --------------------------------------------------------------------------
// <copyright file="DynamicField.cs" company="-">
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

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Tasslehoff.Dynamic
{
    /// <summary>
    /// DynamicField class.
    /// </summary>
    public class DynamicField
    {
        // fields

        /// <summary>
        /// The field builder.
        /// </summary>
        private readonly FieldBuilder fieldBuilder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicField"/> class.
        /// </summary>
        /// <param name="fieldBuilder">The field builder instance</param>
        public DynamicField(FieldBuilder fieldBuilder)
        {
            this.fieldBuilder = fieldBuilder;
        }

        // properties

        /// <summary>
        /// Gets or sets the field builder.
        /// </summary>
        /// <value>
        /// The field builder.
        /// </value>
        public FieldBuilder FieldBuilder
        {
            get
            {
                return this.fieldBuilder;
            }
        }

        // methods

        /// <summary>
        /// Adds an attribute to field.
        /// </summary>
        /// <param name="type">Type of the attribute</param>
        /// <param name="parameterTypes">Parameter types</param>
        /// <param name="parameters">Parameter values</param>
        public void AddAttribute(Type type, Type[] parameterTypes = null, object[] parameters = null)
        {
            this.AddAttribute(
                new CustomAttributeBuilder(
                    type.GetConstructor(parameterTypes ?? new Type[0]),
                    parameters ?? new object[0]
                )
            );
        }

        /// <summary>
        /// Adds an attribute to field.
        /// </summary>
        /// <param name="customAttributeBuilder">Custom attribute builder</param>
        public void AddAttribute(CustomAttributeBuilder customAttributeBuilder)
        {
            this.FieldBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        /// <summary>
        /// Adds property wrapper for the field.
        /// </summary>
        /// <param name="dynamicType">Dynamic type instance</param>
        /// <param name="name">Name of the property</param>
        /// <returns>Dynamic property instance</returns>
        public DynamicProperty ConvertToProperty(DynamicType dynamicType, string name)
        {
            DynamicProperty dynamicProperty = dynamicType.AddProperty(
                name,
                this.FieldBuilder.FieldType,
                PropertyAttributes.HasDefault
            );

            dynamicProperty.CreateGetMethod(dynamicType, this);
            dynamicProperty.CreateSetMethod(dynamicType, this);

            return dynamicProperty;
        }
    }
}
