// --------------------------------------------------------------------------
// <copyright file="VariableHelpers.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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

namespace Tasslehoff.Common.Helpers
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// VariableUtils class.
    /// </summary>
    public static class VariableHelpers
    {
        // methods

        /// <summary>
        /// Checks the and dispose.
        /// </summary>
        /// <param name="variable">The variable</param>
        public static void CheckAndDispose<T>(ref T variable) where T : class, IDisposable
        {
            if (variable != null)
            {
                variable.Dispose();
                variable = null;
            }
        }

        /// <summary>
        /// Gets the member type.
        /// </summary>
        /// <param name="member">The member</param>
        /// <returns>Member type</returns>
        public static Type GetMemberType(MemberInfo member)
        {
            if (member is FieldInfo)
            {
                return (member as FieldInfo).FieldType;
            }
            else if (member is PropertyInfo)
            {
                return (member as PropertyInfo).PropertyType;
            }

            return null;
        }

        /// <summary>
        /// Reads the member value.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="instance">The instance</param>
        /// <param name="enumAsString">Whether serialize enum as string or not</param>
        /// <returns>Value of the field/property instance</returns>
        public static object ReadMemberValue(MemberInfo member, object instance, bool enumAsString = false)
        {
            object value = null;

            if (member is FieldInfo)
            {
                FieldInfo fieldInfo = member as FieldInfo;

                value = fieldInfo.GetValue(instance);

                if (fieldInfo.FieldType.IsEnum)
                {
                    if (enumAsString)
                    {
                        value = value.ToString();
                    }
                    else
                    {
                        value = Convert.ChangeType(value, Enum.GetUnderlyingType(fieldInfo.FieldType));
                    }
                }
            }
            else if (member is PropertyInfo)
            {
                PropertyInfo propertyInfo = member as PropertyInfo;

                value = propertyInfo.GetValue(instance, null);

                if (propertyInfo.PropertyType.IsEnum)
                {
                    if (enumAsString)
                    {
                        value = value.ToString();
                    }
                    else
                    {
                        value = Convert.ChangeType(value, Enum.GetUnderlyingType(propertyInfo.PropertyType));
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Writes the member value.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="instance">The instance</param>
        /// <param name="value">The value</param>
        /// <param name="enumAsString">Whether serialize enum as string or not</param>
        public static void WriteMemberValue(MemberInfo member, object instance, object value, bool enumAsString = false)
        {
            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            Type type;

            if (member is FieldInfo)
            {
                fieldInfo = member as FieldInfo;
                type = Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType;
            }
            else if (member is PropertyInfo)
            {
                propertyInfo = member as PropertyInfo;
                type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            }
            else
            {
                return;
            }

            if (value != null)
            {
                if (type.IsEnum && enumAsString)
                {
                    value = Enum.Parse(type, value.ToString());
                }
                else if (type != value.GetType())
                {
                    value = Convert.ChangeType(value, type);
                }
            }

            if (member is FieldInfo)
            {
                fieldInfo.SetValue(instance, value);
            }
            else if (member is PropertyInfo)
            {
                propertyInfo.SetValue(instance, value);
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>Size of the type</returns>
        public static int GetSize<T>()
        {
            Type type = typeof(T);

            if (type.IsValueType)
            {
                if (type.IsGenericType)
                {
                    // generic value type
                    return Marshal.SizeOf(default(T));
                }
                else
                {
                    // value type
                    return Marshal.SizeOf(type);
                }
            }

            // a reference
            return IntPtr.Size;
        }
    }
}
