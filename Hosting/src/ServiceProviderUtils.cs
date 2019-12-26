// --------------------------------------------------------------------------
// <copyright file="ServiceProviderUtils.cs" company="-">
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
using System.Linq;

namespace Microsoft.Extensions.Hosting {
    public static class ServiceProviderUtils {
        public static object? GetDefaultValueOfType(Type type) {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null) {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static object?[] CallMethodWithServiceProvider(object instance, string methodName, IServiceProvider serviceProvider, IDictionary<Type, object>? fallbackDictionary = null) {
            var instanceType = instance.GetType();
            var methods = instanceType.GetMethods().Where(x => x.Name == methodName).ToArray();
            var returnValues = new object?[methods.Length];

            for (var i = 0; i < methods.Length; i++) {
                var method = methods[i];
                var parameters = method.GetParameters();

                var parameterValues = new object?[parameters.Length];

                for (int j = 0, length = parameterValues.Length; j < length; j++) {
                    var parameterType = parameters[j].ParameterType;
                    var parameterValue = serviceProvider.GetService(parameterType);

                    if (parameterValue != null) {
                        parameterValues[j] = parameterValue;
                        continue;
                    }

                    if (fallbackDictionary != null && fallbackDictionary.ContainsKey(parameterType)) {
                        parameterValues[j] = fallbackDictionary[parameterType];
                        continue;
                    }

                    parameterValues[j] = ServiceProviderUtils.GetDefaultValueOfType(parameterType);
                }

                returnValues[i] = method.Invoke(instance, parameterValues);
            }

            return returnValues;
        }
    }
}
