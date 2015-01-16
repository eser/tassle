// --------------------------------------------------------------------------
// <copyright file="DynamicProperty.cs" company="-">
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
using System.Reflection;
using System.Reflection.Emit;
using OwnDynamicMethod = Tasslehoff.Dynamic.DynamicMethod;

namespace Tasslehoff.Dynamic
{
    /// <summary>
    /// DynamicProperty class.
    /// </summary>
    public class DynamicProperty
    {
        // fields

        /// <summary>
        /// The property builder.
        /// </summary>
        private readonly PropertyBuilder propertyBuilder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProperty"/> class.
        /// </summary>
        /// <param name="propertyBuilder">The property builder instance</param>
        public DynamicProperty(PropertyBuilder propertyBuilder)
        {
            this.propertyBuilder = propertyBuilder;
        }

        // properties

        /// <summary>
        /// Gets or sets the property builder.
        /// </summary>
        /// <value>
        /// The property builder.
        /// </value>
        public PropertyBuilder PropertyBuilder
        {
            get
            {
                return this.propertyBuilder;
            }
        }

        // methods

        /// <summary>
        /// Adds an attribute to property.
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
        /// Adds an attribute to property.
        /// </summary>
        /// <param name="customAttributeBuilder">Custom attribute builder</param>
        public void AddAttribute(CustomAttributeBuilder customAttributeBuilder)
        {
            this.PropertyBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        /// <summary>
        /// Creates a getter method which returns specified field.
        /// </summary>
        /// <param name="dynamicType">Dynamic type instance</param>
        /// <param name="dynamicField">Dynamic field instance</param>
        /// <param name="methodAttributes">Method attributes</param>
        /// <returns>Dynamic method instance</returns>
        public OwnDynamicMethod CreateGetMethod(DynamicType dynamicType, DynamicField dynamicField, MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig)
        {
            OwnDynamicMethod dynamicMethod = dynamicType.AddMethod(
                "get_" + this.PropertyBuilder.Name,
                this.PropertyBuilder.PropertyType,
                new Type[0],
                methodAttributes
            );

            ILGenerator getMethodILGenerator = dynamicMethod.MethodBuilder.GetILGenerator();
            getMethodILGenerator.Emit(OpCodes.Ldarg_0);
            getMethodILGenerator.Emit(OpCodes.Ldfld, dynamicField.FieldBuilder);

            getMethodILGenerator.Emit(OpCodes.Ret);

            this.PropertyBuilder.SetGetMethod(dynamicMethod.MethodBuilder);

            return dynamicMethod;
        }

        /// <summary>
        /// Creates a setter method which sets specified field.
        /// </summary>
        /// <param name="dynamicType">Dynamic type instance</param>
        /// <param name="dynamicField">Dynamic field instance</param>
        /// <param name="methodAttributes">Method attributes</param>
        /// <returns>Dynamic method instance</returns>
        public OwnDynamicMethod CreateSetMethod(DynamicType dynamicType, DynamicField dynamicField, MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig)
        {
            OwnDynamicMethod dynamicMethod = dynamicType.AddMethod(
                "set_" + this.PropertyBuilder.Name,
                null,
                new Type[] { this.PropertyBuilder.PropertyType },
                methodAttributes
            );

            ILGenerator setMethodILGenerator = dynamicMethod.MethodBuilder.GetILGenerator();
            setMethodILGenerator.Emit(OpCodes.Ldarg_0);
            setMethodILGenerator.Emit(OpCodes.Ldarg_1);
            setMethodILGenerator.Emit(OpCodes.Stfld, dynamicField.FieldBuilder);

            setMethodILGenerator.Emit(OpCodes.Ret);

            this.PropertyBuilder.SetSetMethod(dynamicMethod.MethodBuilder);

            return dynamicMethod;
        }
    }
}
