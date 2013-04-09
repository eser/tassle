//
//  LogEntry.cs
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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tasslehoff.Library.Logger
{
    public class LogEntry
    {
        // fields
        private string application;
        private string category;
        private DateTime date;
        private string message;
        private bool isDirect;
        private readonly string threadFrom;
        private LogLevel level;

        // constructors
        public LogEntry ()
        {
            this.threadFrom = Thread.CurrentThread.Name;
        }

        public string Application {
            get {
                return this.application;
            }
            set {
                this.application = value;
            }
        }

        public string Category {
            get {
                return this.category;
            }
            set {
                this.category = value;
            }
        }

        public DateTime Date {
            get {
                return this.date;
            }
            set {
                this.date = value;
            }
        }

        public string Message {
            get {
                return this.message;
            }
            set {
                this.message = value;
            }
        }

        public bool IsDirect {
            get {
                return this.isDirect;
            }
            set {
                this.isDirect = value;
            }
        }

        public string ThreadFrom {
            get {
                return this.threadFrom;
            }
        }

        public LogLevel Level {
            get {
                return this.level;
            }
            set {
                this.level = value;
            }
        }

        // methods
        public string ApplyFormat(string format) {
            string _input = format;
            
            return Regex.Replace(_input, @"{([^:}]*)(:[^}]*)?}", (match) => {
                string _format;
                if(match.Groups.Count >= 2) {
                    _format = match.Groups[2].Value.TrimStart(':');
                }
                else {
                    _format = null;
                }
                
                switch(match.Groups[1].Value) {
                case "application":
                    return this.application ?? string.Empty;
                case "category":
                    return this.category ?? string.Empty;
                case "level":
                    return this.level.ToString();
                case "date":
                case "timestamp":
                    if(_format != null) {
                        return this.date.ToString(_format, CultureInfo.InvariantCulture);
                    }
                    return this.date.ToString(CultureInfo.InvariantCulture);
                case "message":
                    return this.message;
                default:
                    return match.Groups[0].Value;
                }
            });
        }
    }
}

