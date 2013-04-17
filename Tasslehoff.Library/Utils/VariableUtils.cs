// -----------------------------------------------------------------------
// <copyright file="VariableUtils.cs" company="-">
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

namespace Tasslehoff.Library.Utils
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// VariableUtils class.
    /// </summary>
    public static class VariableUtils
    {
        // methods

        /// <summary>
        /// Checks the and dispose.
        /// </summary>
        /// <param name="variable">The variable</param>
        public static void CheckAndDispose(IDisposable variable)
        {
            if (variable != null)
            {
                variable.Dispose();
            }
        }

        /// <summary>
        /// Gets the member type.
        /// </summary>
        /// <param name="member">The member</param>
        /// <returns>Member type</returns>
        public static Type GetMemberType(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                return (member as FieldInfo).FieldType;
            }
            else if (member.MemberType == MemberTypes.Property)
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
        /// <returns>Value of the field/property instance</returns>
        public static object ReadMemberValue(MemberInfo member, object instance)
        {
            object value = null;

            if (member.MemberType == MemberTypes.Field)
            {
                value = (member as FieldInfo).GetValue(instance);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                value = (member as PropertyInfo).GetValue(instance, null);
            }

            return value;
        }

        /// <summary>
        /// Writes the member value.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="instance">The instance</param>
        /// <param name="value">The value</param>
        public static void WriteMemberValue(MemberInfo member, object instance, object value)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                (member as FieldInfo).SetValue(instance, value);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                (member as PropertyInfo).SetValue(instance, value, null);
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
