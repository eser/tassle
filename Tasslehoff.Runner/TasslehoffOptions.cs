// -----------------------------------------------------------------------
// <copyright file="TasslehoffOptions.cs" company="-">
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
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// CommandLineOptions class.
    /// </summary>
    public class TasslehoffOptions
    {
        // fields

        /// <summary>
        /// The config file
        /// </summary>
        private string configFile = null;

        /// <summary>
        /// The working directory
        /// </summary>
        private string workingDirectory = null;

        /// <summary>
        /// The show help
        /// </summary>
        private bool showHelp = false;

        /// <summary>
        /// The verbose mode
        /// </summary>
        private bool verboseMode = false;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TasslehoffOptions" /> class.
        /// </summary>
        public TasslehoffOptions()
        {
        }

        // attributes

        /// <summary>
        /// Gets or sets the config file.
        /// </summary>
        /// <value>
        /// The config file.
        /// </value>
        public string ConfigFile
        {
            get
            {
                return this.configFile;
            }

            set
            {
                this.configFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>
        /// The working directory.
        /// </value>
        public string WorkingDirectory
        {
            get
            {
                return this.workingDirectory;
            }

            set
            {
                this.workingDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show help].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show help]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowHelp
        {
            get
            {
                return this.showHelp;
            }

            set
            {
                this.showHelp = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [verbose mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [verbose mode]; otherwise, <c>false</c>.
        /// </value>
        public bool VerboseMode
        {
            get
            {
                return this.verboseMode;
            }

            set
            {
                this.verboseMode = value;
            }
        }

        // methods

        /// <summary>
        /// Reads instance options from the command line.
        /// </summary>
        /// <param name="args">The args</param>
        /// <returns>Read command line options</returns>
        /// <exception cref="System.ArgumentException">
        /// If one of parameters has errors.
        /// </exception>
        public static TasslehoffOptions FromCommandLine(string[] args)
        {
            TasslehoffOptions runnerOptions = new TasslehoffOptions();
            Queue<string> argsQueue = new Queue<string>(args);

            while (argsQueue.Count > 0)
            {
                string arg = argsQueue.Dequeue();

                switch (arg)
                {
                    case "--config":
                    case "-c":
                        if (argsQueue.Count == 0)
                        {
                            throw new ArgumentException("No input file specified.", "--config");
                        }

                        runnerOptions.ConfigFile = argsQueue.Dequeue();
                        break;
                    case "--working-dir":
                    case "-w":
                        if (argsQueue.Count == 0)
                        {
                            throw new ArgumentException("No working directory specified.", "--working-dir");
                        }

                        runnerOptions.WorkingDirectory = argsQueue.Dequeue();
                        break;
                    case "--help":
                    case "-h":
                    case "-?":
                        runnerOptions.ShowHelp = true;
                        break;
                    case "--verbose":
                    case "-v":
                        runnerOptions.VerboseMode = true;
                        break;
                    default:
                        throw new ArgumentException("Invalid parameter - \"" + arg + "\".", arg);
                }
            }

            return runnerOptions;
        }

        /// <summary>
        /// Shows help on command line parameters.
        /// </summary>
        /// <returns>Help context</returns>
        public static string Help()
        {
            StringBuilder help = new StringBuilder();

            help.AppendLine("Command Line Parameters:");
            help.AppendLine("-c | --config               : Configuration File");
            help.AppendLine("-w | --working-dir          : Working Directory");
            help.AppendLine("-h | --help                 : This help");
            help.AppendLine("-v | --verbose              : Verbose");
            help.AppendLine();
            help.AppendLine("For further information, visit:");
            help.AppendLine("https://github.com/larukedi/tasslehoff");

            return help.ToString();
        }
    }
}
