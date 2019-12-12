// --------------------------------------------------------------------------
// <copyright file="DependencyInjectionUtils.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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

        public static object[] PopulateParameterListWithServiceProvider(MethodInfo methodInfo, IServiceProvider serviceProvider, params object[] args) {
            var parameters = methodInfo.GetParameters();

            var parameterValues = new object[parameters.Length];

            var argsLength = args.Length;

            if (argsLength > 0) {
                args.CopyTo(parameterValues, 0);
            }

            for (int i = argsLength, length = parameterValues.Length; i < length; i++) {
                var parameterType = parameters[i].ParameterType;
                var parameterValue = serviceProvider?.GetService(parameterType);

                if (parameterValue == null) {
                    parameterValues[i] = DependencyInjectionUtils.GetDefaultValueOfType(parameterType);
                    continue;
                }

                parameterValues[i] = parameterValue;
            }

            return parameterValues;
        }

        public static object CallMethodInfoWithServiceProvider(MethodInfo methodInfo, object instance, IServiceProvider serviceProvider, params object[] args) {
            var parameterValues = DependencyInjectionUtils.PopulateParameterListWithServiceProvider(methodInfo, serviceProvider, args);

            var returnValue = methodInfo.Invoke(instance, parameterValues);

            return returnValue;
        }

        public static (string methodName, object returnValue) CallMethodWithServiceProvider(object instance, IEnumerable<string> methodNames, IServiceProvider serviceProvider, params object[] args) {
            var instanceType = instance.GetType();

            foreach (var methodName in methodNames) {
                var methodInfo = instanceType.GetMethod(methodName);

                if (methodInfo == null) {
                    continue;
                }

                var returnValue = DependencyInjectionUtils.CallMethodInfoWithServiceProvider(methodInfo, instance, serviceProvider, args);

                return (methodName, returnValue);
            }

            return (null, null);
        }
    }
}
