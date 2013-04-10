// -----------------------------------------------------------------------
// <copyright file="DataEntityMapper.cs" company="-">
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

namespace Tasslehoff.Library.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Tasslehoff.Library.Collections;

    /// <summary>
    /// DataEntity Mapper class.
    /// </summary>
    [ComVisible(false)]
    public class DataEntityMapper : DictionaryBase<DataEntityFieldAttribute>
    {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityMapper"/> class.
        /// </summary>
        public DataEntityMapper() : base()
        {
        }

        // static methods

        /// <summary>
        /// Reads from class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Deserialized class.</returns>
        public static DataEntityMapper ReadFromClass(Type type)
        {
            DataEntityMapper mappings = new DataEntityMapper();
            
            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MemberInfo member in members)
            {
                if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property)
                {
                    continue;
                }
                
                object[] attributes = member.GetCustomAttributes(typeof(DataEntityFieldAttribute), true);
                foreach (object attribute in attributes)
                {
                    DataEntityFieldAttribute fieldAttribute = (DataEntityFieldAttribute)attribute;
                    
                    if (string.IsNullOrEmpty(fieldAttribute.FieldName))
                    {
                        fieldAttribute.FieldName = member.Name;
                    }
                    
                    fieldAttribute.ClassMember = member;
                    if (fieldAttribute.ClassMember.MemberType == MemberTypes.Field)
                    {
                        fieldAttribute.Type = (member as FieldInfo).FieldType;
                    }
                    else if (fieldAttribute.ClassMember.MemberType == MemberTypes.Property)
                    {
                        fieldAttribute.Type = (member as PropertyInfo).PropertyType;
                    }
                    
                    if (fieldAttribute.UnderlyingType == null)
                    {
                        fieldAttribute.UnderlyingType = fieldAttribute.Type;
                    }

                    mappings.Add(fieldAttribute.FieldName, fieldAttribute);
                }
            }
            
            return mappings;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Deserialized dictionary.</returns>
        public static IDictionary<string, object> GetItem(IDataReader reader)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                
                object fieldValue = reader.GetValue(i);
                if (Convert.IsDBNull(fieldValue))
                {
                    fieldValue = null;
                }
                
                dictionary.Add(fieldName, fieldValue);
            }
            
            return dictionary;
        }

        /// <summary>
        /// Reads the type value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>Value of the field/property instance.</returns>
        public static object ReadTypeValue(DataEntityFieldAttribute field, object instance)
        {
            object value = null;
            if (field.ClassMember.MemberType == MemberTypes.Field)
            {
                value = (field.ClassMember as FieldInfo).GetValue(instance);
            }
            else if (field.ClassMember.MemberType == MemberTypes.Property)
            {
                value = (field.ClassMember as PropertyInfo).GetValue(instance, null);
            }

            return value;
        }

        /// <summary>
        /// Writes the type value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public static void WriteTypeValue(DataEntityFieldAttribute field, object instance, object value)
        {
            if (field.ClassMember.MemberType == MemberTypes.Field)
            {
                (field.ClassMember as FieldInfo).SetValue(instance, value);
            }
            else if (field.ClassMember.MemberType == MemberTypes.Property)
            {
                (field.ClassMember as PropertyInfo).SetValue(instance, value, null);
            }
        }

        // methods

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns>Deserialized class.</returns>
        public T GetItem<T>(IDataReader reader) where T : IDataEntity, new()
        {
            T instance = new T();
            
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                
                if (!this.ContainsKey(fieldName))
                {
                    continue;
                }
                
                object fieldValue = reader.GetValue(i);
                if (Convert.IsDBNull(fieldValue))
                {
                    fieldValue = null;
                }
                
                if (this[fieldName].GetFunction != null)
                {
                    fieldValue = this[fieldName].GetFunction(fieldValue);
                }
                
                DataEntityMapper.WriteTypeValue(this[fieldName], instance, fieldValue);
            }
            
            return instance;
        }
    }
}
