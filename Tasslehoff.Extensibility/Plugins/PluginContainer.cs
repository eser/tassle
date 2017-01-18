// --------------------------------------------------------------------------
// <copyright file="PluginContainer.cs" company="-">
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
using System.Collections.Generic;
using Tasslehoff.Services;

namespace Tasslehoff.Extensibility.Plugins
{
    /// <summary>
    /// PluginContainer class.
    /// </summary>
    public class PluginContainer : ServiceContainer
    {
        // fields

        /// <summary>
        /// The extension manager
        /// </summary>
        private readonly ExtensionFinder extensionFinder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContainer" /> class.
        /// </summary>
        /// <param name="extensionFinder">The extension finder</param>
        public PluginContainer(ExtensionFinder extensionFinder) : base()
        {
            this.extensionFinder = extensionFinder;
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "PluginContainer";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the extension finder.
        /// </summary>
        /// <value>
        /// The extension finder.
        /// </value>
        public ExtensionFinder ExtensionFinder
        {
            get
            {
                return this.extensionFinder;
            }
        }

        // methods

        /// <summary>
        /// Invokes events will be occurred during the service start.
        /// </summary>
        protected override void ServiceStart()
        {
            IEnumerable<Type> types = this.extensionFinder.SearchInterface(typeof(IPlugin));
            foreach (Type type in types)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                this.AddChild(plugin);
            }
        }

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected override void ServiceStop()
        {
            foreach (Service service in this.Children)
            {
                service.Dispose();
            }

            this.Children.Clear();
        }
    }
}
