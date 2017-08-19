// --------------------------------------------------------------------------
// <copyright file="DependencyInjectionUtils.cs" company="-">
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

using System;
using System.Reflection;

namespace Tassle.Functions {
    /// <summary>
    /// DependencyInjectionUtils class.
    /// </summary>
    public static class DependencyInjectionUtils {
        public static object GetDefaultValueOfType(Type type) {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null) {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static object[] PopulateParameterListWithServiceProvider(MethodInfo methodInfo, IServiceProvider serviceProvider) {
            var parameters = methodInfo.GetParameters();

            var parameterValues = new object[parameters.Length];

            for (int i = 0, length = parameterValues.Length; i < length; i++) {
                var parameterType = parameters[i].ParameterType;
                var parameterValue = serviceProvider.GetService(parameterType);

                if (parameterValue == null) {
                    parameterValues[i] = DependencyInjectionUtils.GetDefaultValueOfType(parameterType);
                    continue;
                }

                parameterValues[i] = parameterValue;
            }

            return parameterValues;
        }

        public static object CallMethodWithServiceProvider(object instance, string methodName, IServiceProvider serviceProvider) {
            var instanceType = instance.GetType();
            var methodInfo = instanceType.GetMethod(methodName);

            if (methodInfo == null) {
                return null;
            }

            var parameterValues = DependencyInjectionUtils.PopulateParameterListWithServiceProvider(methodInfo, serviceProvider);
            var returnValue = methodInfo.Invoke(instance, parameterValues);

            return returnValue;
        }
    }
}
