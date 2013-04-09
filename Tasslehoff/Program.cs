//
//  Program.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using Tasslehoff.Runner;
using Tasslehoff.Globals;
using Tasslehoff.Library.Config;

namespace Tasslehoff
{
	class MainClass
	{
        // constants
        public const string CONFIG_FILENAME = "instanceConfig.json"; 

        // entry point
		public static void Main (string[] args)
		{
            string _configFile = null;

            try {
                Queue<string> _argsQueue = new Queue<string>(args);

                while(_argsQueue.Count > 0) {
                    string _arg = _argsQueue.Dequeue();

                    switch(_arg) {
                    case "--config":
                    case "-c":
                        if(_argsQueue.Count == 0) {
                            throw new ArgumentException("No input file specified.", "--config");
                        }

                        _configFile = _argsQueue.Dequeue();
                        break;
                    default:
                        throw new ArgumentException("Invalid parameter - \"" + _arg + "\".", _arg);
                    }
                }
            }
            catch(ArgumentException _ex) {
                Console.WriteLine(_ex.Message);
                return;
            }

            InstanceConfig _config;
            if(_configFile != null) {
                if(!File.Exists(_configFile)) {
                    throw new ArgumentException("File not found or inaccessible - \"" + _configFile + "\".", "--config");
                }

                Stream _fileStream = File.OpenRead(_configFile);
                _config = ConfigSerializer.Load<InstanceConfig>(_fileStream);
            }
            else if(File.Exists(MainClass.CONFIG_FILENAME)) {
                Stream _fileStream = File.OpenRead(CONFIG_FILENAME);
                _config = ConfigSerializer.Load<InstanceConfig>(_fileStream);
            }
            else {
                _config = new InstanceConfig();
                ConfigSerializer.Reset(_config);
                ConfigSerializer.Save(File.OpenWrite(MainClass.CONFIG_FILENAME), _config);
            }

            Instance _instance = new Instance(_config);
		}
	}
}
