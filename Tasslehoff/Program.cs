// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
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

namespace Tasslehoff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Tasslehoff.Globals;
    using Tasslehoff.Library.Config;
    using Tasslehoff.Runner;

    /// <summary>
    /// Entry point class of the project.
    /// </summary>
    public class Program
    {
        // constants

        /// <summary>
        /// Filename of the default configuration file
        /// </summary>
        public const string ConfigFilename = "instanceConfig.json"; 

        // methods

        /// <summary>
        /// Entry point method of the project.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <exception cref="System.ArgumentException">
        /// Occurs when one of the command line argument has problems.
        /// </exception>
        public static void Main(string[] args)
        {
            string configFile = null;

            try
            {
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

                        configFile = argsQueue.Dequeue();
                        break;
                    default:
                        throw new ArgumentException("Invalid parameter - \"" + arg + "\".", arg);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            InstanceConfig config;
            if (configFile != null)
            {
                if (!File.Exists(configFile))
                {
                    throw new ArgumentException("File not found or inaccessible - \"" + configFile + "\".", "--config");
                }

                Stream fileStream = File.OpenRead(configFile);
                config = ConfigSerializer.Load<InstanceConfig>(fileStream);
            }
            else if (File.Exists(Program.ConfigFilename))
            {
                Stream fileStream = File.OpenRead(ConfigFilename);
                config = ConfigSerializer.Load<InstanceConfig>(fileStream);
            }
            else
            {
                config = new InstanceConfig();
                ConfigSerializer.Reset(config);
                ConfigSerializer.Save(File.OpenWrite(Program.ConfigFilename), config);
            }

            Instance instance = new Instance(config);
            instance.Start();

            Console.ReadLine();

            instance.Stop();
            //// instance.Dispose();
        }
    }
}
