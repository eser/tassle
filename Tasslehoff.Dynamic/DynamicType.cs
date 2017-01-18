// --------------------------------------------------------------------------
// <copyright file="DynamicType.cs" company="-">
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
using OwnDynamicMethod = Tasslehoff.Dynamic.DynamicMethod;

namespace Tasslehoff.Dynamic
{
    /// <summary>
    /// DynamicType class.
    /// </summary>
    public class DynamicType
    {
        // fields

        /// <summary>
        /// The type builder.
        /// </summary>
        private readonly TypeBuilder typeBuilder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicType"/> class.
        /// </summary>
        /// <param name="typeBuilder">The type builder instance</param>
        public DynamicType(TypeBuilder typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        // properties

        /// <summary>
        /// Gets or sets the type builder.
        /// </summary>
        /// <value>
        /// The type builder.
        /// </value>
        public TypeBuilder TypeBuilder
        {
            get
            {
                return this.typeBuilder;
            }
        }

        // methods

        /// <summary>
        /// Adds an attribute to type.
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
        /// Adds an attribute to type.
        /// </summary>
        /// <param name="customAttributeBuilder">Custom attribute builder</param>
        public void AddAttribute(CustomAttributeBuilder customAttributeBuilder)
        {
            this.TypeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        /// <summary>
        /// Adds an interface implementation to type.
        /// </summary>
        /// <param name="interfaceType">The interface type</param>
        public void AddInterface(Type interfaceType)
        {
            this.TypeBuilder.AddInterfaceImplementation(interfaceType);
        }

        /// <summary>
        /// Adds a field to type.
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="type">Type of the field</param>
        /// <param name="fieldAttributes">Field attributes</param>
        /// <returns>Field instance</returns>
        public DynamicField AddField(string name, Type type, FieldAttributes fieldAttributes = FieldAttributes.Public)
        {
            FieldBuilder fieldBuilder = this.TypeBuilder.DefineField(name, type, fieldAttributes);

            return new DynamicField(fieldBuilder);
        }

        /// <summary>
        /// Adds a property to type.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="propertyAttributes">Property attributes</param>
        /// <returns>Property instance</returns>
        public DynamicProperty AddProperty(string name, Type type, PropertyAttributes propertyAttributes = PropertyAttributes.None)
        {
            // DynamicField fieldBuilder = this.AddField("_" + name, type, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = this.TypeBuilder.DefineProperty(name, propertyAttributes, type, new Type[] { type });

            return new DynamicProperty(propertyBuilder);
        }

        /// <summary>
        /// Adds a method to type.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="parameterTypes">Parameter types</param>
        /// <param name="methodAttributes">Method attributes</param>
        /// <returns>Method instance</returns>
        public OwnDynamicMethod AddMethod(string name, Type returnType = null, Type[] parameterTypes = null, MethodAttributes methodAttributes = MethodAttributes.Public)
        {
            MethodBuilder methodBuilder = this.TypeBuilder.DefineMethod(
                name,
                methodAttributes,
                returnType,
                parameterTypes ?? new Type[0]
            );

            return new OwnDynamicMethod(methodBuilder);
        }

        /// <summary>
        /// Finalizes the type.
        /// </summary>
        /// <returns>Created type</returns>
        public Type Finalize()
        {
            return this.TypeBuilder.CreateType();
        }
    }
}
