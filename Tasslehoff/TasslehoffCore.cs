// --------------------------------------------------------------------------
// <copyright file="TasslehoffCore.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
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
using System.Globalization;
using System.Threading;
using Tasslehoff.Adapters;
using Tasslehoff.Adapters.IronMQ;
using Tasslehoff.Adapters.Memcached;
using Tasslehoff.Adapters.RabbitMQ;
using Tasslehoff.Adapters.Redis;
using Tasslehoff.DataAccess;
using Tasslehoff.Extensibility;
using Tasslehoff.Extensibility.Plugins;
using Tasslehoff.Logging;
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
        /// The first initialize
        /// </summary>
        private bool firstInit = true;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TasslehoffCore" /> class.
        /// </summary>
        /// <param name="configuration">The core configuration</param>
        public TasslehoffCore(TasslehoffCoreConfig configuration = null)
            : base()
        {
            // initialization
            this.configuration = configuration;

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
            if (!this.firstInit)
            {
                return;
            }

            this.firstInit = false;

            if (this.Configuration != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(this.Configuration.Culture);
            }

            this.Log.Write(LogLevel.Info, "Tasslehoff 0.9.7  (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.");
            this.Log.Write(LogLevel.Info, "This program is free software under the terms of the GPL v3 or later.");
            this.Log.Write(LogLevel.Info, string.Empty);

            if (this.Configuration != null)
            {
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
            }

            this.Log.Write(LogLevel.Info, string.Format("{0} extensions found:", this.ExtensionFinder.Assemblies.Count));
            foreach (string assemblyName in this.ExtensionFinder.Assemblies.Keys)
            {
                this.Log.Write(LogLevel.Info, string.Format("- {0}", assemblyName));
            }
            this.Log.Write(LogLevel.Info, string.Empty);

            if (this.Configuration != null)
            {
                if (!string.IsNullOrEmpty(this.Configuration.RabbitMQAddress))
                {
                    IQueueManager rabbitMq = new RabbitMQConnection(this.Configuration.RabbitMQAddress);
                    this.AddChild(rabbitMq);
                }

                if (!string.IsNullOrEmpty(this.Configuration.IronMQProjectId))
                {
                    IQueueManager ironMq = new IronMQConnection(this.Configuration.IronMQProjectId, this.Configuration.IronMQToken);
                    this.AddChild(ironMq);
                }

                if (!string.IsNullOrEmpty(this.Configuration.MemcachedAddress))
                {
                    ICacheManager memcached = new MemcachedConnection(this.Configuration.MemcachedAddress);
                    this.AddChild(memcached);
                }

                if (!string.IsNullOrEmpty(this.Configuration.RedisAddress))
                {
                    ICacheManager redis = new RedisConnection(this.Configuration.RedisAddress);
                    this.AddChild(redis);
                }

                if (!string.IsNullOrEmpty(this.Configuration.ElasticSearchAddress))
                {
                    ICacheManager elasticSearch = new ElasticSearchConnection(new Uri(this.Configuration.ElasticSearchAddress));
                    this.AddChild(elasticSearch);
                }
            }
        }

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected override void ServiceStop()
        {
            this.TaskManager.Clear();

            ICacheManager cacheManager = this.Find<ICacheManager>(false);
            if (cacheManager != null)
            {
                this.Children.Remove(cacheManager);
            }

            IQueueManager queueManager = this.Find<IQueueManager>(false);
            if (queueManager != null)
            {
                this.Children.Remove(queueManager);
            }
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
            this.Log.Write(LogLevel.Info, "Loaded Plugins:");
            foreach (IServiceDefined service in this.PluginContainer.GetDefinedChildrenOnly())
            {
                this.Log.Write(LogLevel.Info, string.Format("- {0} {1}", service.Name, service.Description));
            }
            this.Log.Write(LogLevel.Info, string.Format("{0} total", this.PluginContainer.Children.Count));
            this.Log.Write(LogLevel.Info, string.Empty);
        }
    }
}
