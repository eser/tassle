// -----------------------------------------------------------------------
// <copyright file="Tasslehoff.cs" company="-">
// Copyright (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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

namespace Tasslehoff
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using Adapters;
    using Library.Cron;
    using Library.DataAccess;
    using Library.Plugins;
    using Library.Services;
    using Library.Utils;

    /// <summary>
    /// Tasslehoff class.
    /// </summary>
    public class Tasslehoff : ServiceContainer
    {
        // fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static Tasslehoff instance = null;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly TasslehoffConfig configuration;

        /// <summary>
        /// The output
        /// </summary>
        private readonly TextWriter output;

        /// <summary>
        /// The database manager
        /// </summary>
        private readonly DatabaseManager databaseManager;

        /// <summary>
        /// The cron manager
        /// </summary>
        private readonly CronManager cronManager;

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

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasslehoff" /> class.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <param name="output">The output</param>
        public Tasslehoff(TasslehoffConfig configuration, TextWriter output)
            : base()
        {
            // singleton pattern
            if (Tasslehoff.instance == null)
            {
                Tasslehoff.instance = this;
            }

            // initialization
            this.configuration = configuration;
            this.output = output;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(configuration.Culture);

            this.output.WriteLine("Tasslehoff 1.1.0  (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.");
            this.output.WriteLine("This program is free software under the terms of the GPL v3 or later.");
            this.output.WriteLine();

            this.databaseManager = new DatabaseManager();

            if (!string.IsNullOrEmpty(this.configuration.DatabaseConnectionString))
            {
                this.databaseManager.Connections.Add(
                    this.databaseManager.DefaultDatabaseKey,
                    new DatabaseManagerConnection()
                    {
                        Driver = this.configuration.DatabaseDriver,
                        ConnectionString = this.configuration.DatabaseConnectionString,
                    }
                );
            }

            this.cronManager = new CronManager();
            this.AddChild(this.cronManager);

            this.extensionFinder = new ExtensionFinder();
            this.AddChild(this.extensionFinder);

            // search for extensions
            if (this.configuration.ExtensionPaths != null)
            {
                foreach (string extensionPath in this.configuration.ExtensionPaths)
                {
                    this.extensionFinder.SearchFiles(extensionPath);
                }
            }
            this.output.WriteLine("{0} extensions found.", this.extensionFinder.Assemblies.Count);

            this.pluginContainer = new PluginContainer(this.extensionFinder);
            this.AddChild(this.pluginContainer);

            this.OnStartWithChildren += this.Tasslehoff_OnStartWithChildren;

            if (this.configuration.VerboseMode)
            {
                this.output.WriteLine("Working Directory: {0}", this.configuration.WorkingDirectory);
                this.output.WriteLine();
            }
        }

        // properties

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static Tasslehoff Instance
        {
            get
            {
                return Tasslehoff.instance;
            }
        }

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
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public TasslehoffConfig Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// Gets the output.
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
        /// Gets the cron manager.
        /// </summary>
        /// <value>
        /// The cron manager.
        /// </value>
        public CronManager CronManager
        {
            get
            {
                return this.cronManager;
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
        /// Services the start.
        /// </summary>
        protected override void ServiceStart()
        {
        }

        /// <summary>
        /// Services the stop.
        /// </summary>
        protected override void ServiceStop()
        {
            this.CronManager.Clear();

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
            if (this.Configuration.VerboseMode)
            {
                this.Output.WriteLine("Loaded Plugins:");
                foreach (IService service in this.PluginContainer.Children.Values)
                {
                    this.Output.WriteLine("- {0} {1}", service.Name, service.Description);
                }
                this.Output.WriteLine("{0} total", this.PluginContainer.Children.Count);
                this.Output.WriteLine();
            }
        }
    }
}
