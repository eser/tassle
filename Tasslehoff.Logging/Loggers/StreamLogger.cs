// --------------------------------------------------------------------------
// <copyright file="StreamLogger.cs" company="-">
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

using System.IO;
using System.Text;

namespace Tasslehoff.Logging.Loggers
{
    /// <summary>
    /// StreamLogger class.
    /// </summary>
    public class StreamLogger
    {
        // fields

        /// <summary>
        /// The output stream
        /// </summary>
        private readonly Stream outputStream;

        /// <summary>
        /// The output encoding
        /// </summary>
        private Encoding outputEncoding;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamLogger"/> class.
        /// </summary>
        /// <param name="output">The output</param>
        /// <param name="encoding">The encoding</param>
        public StreamLogger(Stream output, Encoding encoding = null)
        {
            this.outputStream = output;
            this.outputEncoding = encoding ?? Encoding.Default;
        }

        // properties

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        /// <value>
        /// The output stream.
        /// </value>
        public Stream OutputStream
        {
            get
            {
                return this.outputStream;
            }
        }

        /// <summary>
        /// Gets or sets the output encoding.
        /// </summary>
        /// <value>
        /// The output encoding.
        /// </value>
        public Encoding OutputEncoding
        {
            get
            {
                return this.outputEncoding;
            }
            set
            {
                this.outputEncoding = value;
            }
        }

        // methods

        /// <summary>
        /// Attachs itself to the log system.
        /// </summary>
        public void Attach()
        {
            LoggerContext.Current.LogEntryPopped += this.OnLogEntryPopped;
        }

        /// <summary>
        /// Detachs itself from the log system.
        /// </summary>
        public void Detach()
        {
            LoggerContext.Current.LogEntryPopped -= this.OnLogEntryPopped;
        }

        /// <summary>
        /// Calls when an log entry popped
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="entry">The log entry</param>
        private void OnLogEntryPopped(object sender, LogEntry entry)
        {
            var content = LoggerContext.Current.Formatter.Apply(entry);

            var bytes = this.outputEncoding.GetBytes(content);
            this.outputStream.Write(bytes, 0, bytes.Length);

            if (entry.Flags.HasFlag(LogFlags.InstantFlush))
            {
                this.outputStream.Flush();
            }
        }
    }
}
