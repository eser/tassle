// --------------------------------------------------------------------------
// <copyright file="TasslehoffCore.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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
using System.Globalization;
using System.IO;
using System.Threading;
using Tasslehoff.Adapters;
using Tasslehoff.DataAccess;
using Tasslehoff.Extensibility;
using Tasslehoff.Extensibility.Plugins;
using Tasslehoff.Services;
using Tasslehoff.Tasks;

namespace Tasslehoff
{
    /// <summary>
    /// TasslehoffCore class.
    /// </summary>
    public class TasslehoffCore : ServiceContainer
    {
        // fields

        /// <summary>
        /// The core configuration.
        /// </summary>
        private TasslehoffCoreConfig configuration;

        /// <summary>
        /// The output
        /// </summary>
        private TextWriter output;

        /// <summary>
        /// The database manager
        /// </summary>
        private readonly DatabaseManager databaseManager;

        /// <summary>
        /// The task manager
        /// </summary>
        private readonly TaskManager taskManager;

        /// <summary>
        /// The extension finder
        /// </summary>
        private readonly ExtensionFinder extensionFinder;

        /// <summary>
        /// The plugin container
        /// </summary>
        private readonly PluginContainer pluginContainer;

        /// <summary>
        /// The queue manager
        /// </summary>
        private IQueueManager queueManager = null;

        /// <summary>
        /// The cache manager
        /// </summary>
        private ICacheManager cacheManager = null;

        /// <summary>
        /// The first initialize
        /// </summary>
        private bool firstInit = true;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TasslehoffCore" /> class.
        /// </summary>
        /// <param name="configuration">The core configuration</param>
        /// <param name="output">The output</param>
        public TasslehoffCore(TasslehoffCoreConfig configuration = null, TextWriter output = null)
            : base()
        {
            // initialization
            this.configuration = configuration;
            this.output = output;

            this.databaseManager = new DatabaseManager();

            this.taskManager = new TaskManager();
            this.AddChild(this.taskManager);

            this.extensionFinder = new ExtensionFinder();
            this.AddChild(this.extensionFinder);

            this.pluginContainer = new PluginContainer(this.extensionFinder);
            this.AddChild(this.pluginContainer);

            this.OnStartWithChildren += this.Tasslehoff_OnStartWithChildren;
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
                return "Tasslehoff Core";
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
        /// Gets and sets the core configuration.
        /// </summary>
        /// <value>
        /// The core configuration.
        /// </value>
        public TasslehoffCoreConfig Configuration
        {
            get
            {
                return this.configuration;
            }
            set
            {
                this.configuration = value;
            }
        }

        /// <summary>
        /// Gets and sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public TextWriter Output
        {
            get
            {
                return this.output;
            }
            set
            {
                this.output = value;
            }
        }

        /// <summary>
        /// Gets the database manager.
        /// </summary>
        /// <value>
        /// The database manager.
        /// </value>
        public DatabaseManager DatabaseManager
        {
            get
            {
                return this.databaseManager;
            }
        }

        /// <summary>
        /// Gets the task manager.
        /// </summary>
        /// <value>
        /// The task manager.
        /// </value>
        public TaskManager TaskManager
        {
            get
            {
                return this.taskManager;
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

        /// <summary>
        /// Gets the plugin container.
        /// </summary>
        /// <value>
        /// The plugin container.
        /// </value>
        public PluginContainer PluginContainer
        {
            get
            {
                return this.pluginContainer;
            }
        }

        /// <summary>
        /// Gets the queue manager.
        /// </summary>
        /// <value>
        /// The queue manager.
        /// </value>
        public IQueueManager QueueManager
        {
            get
            {
                return this.queueManager;
            }
            protected set
            {
                this.queueManager = value;
            }
        }

        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        /// <value>
        /// The cache manager.
        /// </value>
        public ICacheManager CacheManager
        {
            get
            {
                return this.cacheManager;
            }
            protected set
            {
                this.cacheManager = value;
            }
        }

        // methods

        /// <summary>
        /// Adds a task to task manager.
        /// </summary>
        /// <param name="taskItem">Task item which is going to be added</param>
        public void AddTask(TaskItem taskItem)
        {
            string key = string.Format("Task{0}", (this.TaskManager.Items.Count + 1));

            this.TaskManager.Add(key, taskItem);
        }

        /// <summary>
        /// Invokes events will be occurred during the service start.
        /// </summary>
        protected override void ServiceStart()
        {
            if (!firstInit)
            {
                return;
            }

            firstInit = false;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(this.Configuration.Culture);

            if (this.Output != null)
            {
                this.Output.WriteLine("Tasslehoff 0.9.4  (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.");
                this.Output.WriteLine("This program is free software under the terms of the GPL v3 or later.");
                this.Output.WriteLine();
            }

            if (!string.IsNullOrEmpty(this.Configuration.DatabaseConnectionString))
            {
                this.DatabaseManager.Connections.Add(
                    this.DatabaseManager.DefaultDatabaseKey,
                    new DatabaseManagerConnection()
                    {
                        ProviderName = this.Configuration.DatabaseProviderName,
                        ConnectionString = this.Configuration.DatabaseConnectionString,
                    }
                );
            }

            // search for extensions
            if (this.Configuration.ExtensionPaths != null)
            {
                this.ExtensionFinder.SearchStructured(this.Configuration.ExtensionPaths);
            }

            if (this.Output != null && this.Configuration.VerboseMode)
            {
                this.Output.WriteLine("{0} extensions found:", this.ExtensionFinder.Assemblies.Count);
                foreach (string assemblyName in this.ExtensionFinder.Assemblies.Keys)
                {
                    this.Output.WriteLine("- {0}", assemblyName);
                }
                this.Output.WriteLine();
            }
        }

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected override void ServiceStop()
        {
            this.TaskManager.Clear();
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected override void OnDispose(bool releaseManagedResources)
        {
            base.OnDispose(releaseManagedResources);
        }

        /// <summary>
        /// Handles the OnStartWithChildren event of the Tasslehoff control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Tasslehoff_OnStartWithChildren(object sender, EventArgs e)
        {
            if (this.Output != null && this.Configuration.VerboseMode)
            {
                this.Output.WriteLine("Loaded Plugins:");
                foreach (IServiceDefined service in this.PluginContainer.GetDefinedChildrenOnly())
                {
                    this.Output.WriteLine("- {0} {1}", service.Name, service.Description);
                }
                this.Output.WriteLine("{0} total", this.PluginContainer.Children.Count);
                this.Output.WriteLine();
            }
        }
    }
}
