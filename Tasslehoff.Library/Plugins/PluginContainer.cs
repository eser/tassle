// -----------------------------------------------------------------------
// <copyright file="PluginContainer.cs" company="-">
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

namespace Tasslehoff.Library.Plugins
{
    using System;
    using System.Collections.Generic;
    using Tasslehoff.Library.Extensions;
    using Tasslehoff.Library.Services;

    /// <summary>
    /// PluginContainer class.
    /// </summary>
    public class PluginContainer : ServiceContainer
    {
        // fields

        /// <summary>
        /// The extension manager
        /// </summary>
        private readonly ExtensionManager extensionManager;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContainer" /> class.
        /// </summary>
        /// <param name="extensionManager">The extension manager</param>
        public PluginContainer(ExtensionManager extensionManager) : base()
        {
            this.extensionManager = extensionManager;
        }

        // attributes

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
        /// Gets the extension manager.
        /// </summary>
        /// <value>
        /// The extension manager.
        /// </value>
        public ExtensionManager ExtensionManager
        {
            get
            {
                return this.extensionManager;
            }
        }

        // methods

        /// <summary>
        /// Services the start.
        /// </summary>
        protected override void ServiceStart()
        {
            IEnumerable<Type> types = this.extensionManager.SearchInterface(typeof(IPlugin));
            foreach (Type type in types)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                this.AddChild(plugin);
            }
        }

        /// <summary>
        /// Services the stop.
        /// </summary>
        protected override void ServiceStop()
        {
            foreach (Service service in this.Children.Values)
            {
                service.Dispose();
            }

            this.Children.Clear();
        }
    }
}
