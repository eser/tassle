// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerFactoryExtensions.cs" company="-">
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

        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, TelnetLoggerSettingsInterface settings) {
            factory.AddProvider(new TelnetLoggerProvider(serviceProvider, settings));

            return factory;
        }

        public static ILoggerFactory AddTelnet(this ILoggerFactory factory, IServiceProvider serviceProvider, IConfiguration configuration) {
            var settings = new ConfigurationTelnetLoggerSettings(configuration);

            return factory.AddTelnet(serviceProvider, settings);
        }
    }
}
