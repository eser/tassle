// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerFactoryExtensions.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Tassle.Telnet;

namespace Tassle.Logging.Telnet {
    public static class TelnetLoggerFactoryExtensions {
        // methods

        /// <summary>
        /// Adds a telnet logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider) {
            return factory.AddTelnet(serviceProvider, includeScopes: false);
        }

        /// <summary>
        /// Adds a telnet logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, bool includeScopes) {
            factory.AddTelnet(serviceProvider, (n, l) => l >= LogLevel.Information, includeScopes);

            return factory;
        }

        /// <summary>
        /// Adds a telnet logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, LogLevel minLevel) {
            factory.AddTelnet(serviceProvider, minLevel, includeScopes: false);

            return factory;
        }

        /// <summary>
        /// Adds a telnet logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, LogLevel minLevel, bool includeScopes) {
            factory.AddTelnet(serviceProvider, (category, logLevel) => logLevel >= minLevel, includeScopes);

            return factory;
        }

        /// <summary>
        /// Adds a telnet logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filter"></param>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter) {
            factory.AddTelnet(serviceProvider, filter, includeScopes: false);

            return factory;
        }

        /// <summary>
        /// Adds a telnet logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filter"></param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, bool includeScopes) {
            factory.AddProvider(new TelnetLoggerProvider(serviceProvider, filter, includeScopes));

            return factory;
        }

        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, ITelnetLoggerSettings settings) {
            factory.AddProvider(new TelnetLoggerProvider(serviceProvider, settings));

            return factory;
        }

        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, IConfiguration configuration) {
            var settings = new ConfigurationTelnetLoggerSettings(configuration);

            return factory.AddTelnet(serviceProvider, settings);
        }
    }
}
