// --------------------------------------------------------------------------
// <copyright file="LogFormatter.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Tasslehoff.Logging
{
    /// <summary>
    /// A LogFormatter instance.
    /// </summary>
    public class LogFormatter
    {
        // fields

        /// <summary>
        /// The default format
        /// </summary>
        private string defaultFormat;

        /// <summary>
        /// Formats
        /// </summary>
        private Dictionary<string, string> formats; 

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFormatter"/> class.
        /// </summary>
        public LogFormatter()
        {
            this.defaultFormat = "default";

            this.formats = new Dictionary<string, string>()
            {
                { this.defaultFormat, "[{application:12}:{thread}] {category:25} {severity:8} {date} {message} {exception}" }
            };
        }

        // properties

        /// <summary>
        /// Gets or sets the default format.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string DefaultFormat
        {
            get
            {
                return this.defaultFormat;
            }
            set
            {
                this.defaultFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets formats.
        /// </summary>
        /// <value>
        /// Formats.
        /// </value>
        public Dictionary<string, string> Formats
        {
            get
            {
                return this.formats;
            }
            set
            {
                this.formats = value;
            }
        }

        // methods

        /// <summary>
        /// Applies the default format to a log entry.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>Formatted message</returns>
        public string Apply(LogEntry entry)
        {
            return this.Apply(this.defaultFormat, entry);
        }

        /// <summary>
        /// Applies a format to a log entry.
        /// </summary>
        /// <param name="format">The format</param>
        /// <param name="entry">The log entry</param>
        /// <returns>Formatted message</returns>
        public string Apply(string format, LogEntry entry)
        {
            return this.ApplyCustom(this.formats[format], entry);
        }

        /// <summary>
        /// Applies a custom format to a log entry.
        /// </summary>
        /// <param name="input">The format string</param>
        /// <param name="entry">The log entry</param>
        /// <param name="forceNoNewLine">Force no new line</param>
        /// <returns>Formatted message</returns>
        public string ApplyCustom(string input, LogEntry entry, bool forceNoNewLine = false)
        {
            string formatted;

            if (entry.Flags.HasFlag(LogFlags.Direct))
            {
                formatted = entry.Message;
            }
            else
            {
                formatted = Regex.Replace(
                    input,
                    @"{([^:}]*)(:[^}]*)?}",
                    (match) =>
                    {
                        string format;
                        if (match.Groups.Count >= 2)
                        {
                            format = match.Groups[2].Value.TrimStart(':');
                        }
                        else
                        {
                            format = null;
                        }

                        string text;
                        var formatUsed = false;

                        switch (match.Groups[1].Value)
                        {
                            case "application":
                                text = entry.Application ?? string.Empty;
                                break;
                            case "thread":
                                text = entry.ThreadFrom ?? string.Empty;
                                break;
                            case "category":
                                text = entry.Category ?? string.Empty;
                                break;
                            case "severity":
                            case "level":
                                text = entry.Severity.ToString("G").ToUpperInvariant();
                                break;
                            case "date":
                            case "timestamp":
                                formatUsed = true;

                                var dateFormat = format ?? "dd/MM/yyyy HH:mm:ss";
                                text = entry.Date.ToString(dateFormat, CultureInfo.InvariantCulture);
                                break;
                            case "message":
                                text = entry.Message;
                                break;
                            case "exception":
                                if (entry.Exception != null)
                                {
                                    var exceptionText = new StringBuilder();
                                    exceptionText.Append("Exception: ");
                                    exceptionText.AppendLine(entry.Exception.GetType().Name);

                                    exceptionText.Append("Message: ");
                                    exceptionText.AppendLine(entry.Exception.Message);

                                    exceptionText.Append("Source: ");
                                    exceptionText.AppendLine(entry.Exception.Source);

                                    exceptionText.AppendLine("=== Stack Trace ===");
                                    exceptionText.AppendLine(entry.Exception.StackTrace);

                                    text = exceptionText.ToString();
                                    break;
                                }

                                text = string.Empty;
                                break;
                            default:
                                text = match.Groups[0].Value;
                                break;
                        }

                        if (!formatUsed && !string.IsNullOrEmpty(format))
                        {
                            return text.PadRight(int.Parse(format));
                        }

                        return text;
                    });
            }

            var output = new StringBuilder();
            var newNewLine = Environment.NewLine + "-- ";
            output.Append(formatted.Replace(Environment.NewLine, newNewLine));

            if (!forceNoNewLine && !entry.Flags.HasFlag(LogFlags.NoNewLine))
            {
                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
