// -----------------------------------------------------------------------
// <copyright file="StreamLogger.cs" company="-">
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

namespace Tasslehoff.Library.Logger
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// StreamLogger class.
    /// </summary>
    public class StreamLogger : Logger
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
        public StreamLogger(Stream output, Encoding encoding = null) : base()
        {
            this.outputStream = output;
            this.outputEncoding = encoding ?? Encoding.Default;
        }

        // attributes

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
        /// Writes the log.
        /// </summary>
        /// <param name="logEntry">The log entry</param>
        /// <returns>
        /// Is written or not.
        /// </returns>
        protected override bool WriteLog(LogEntry logEntry)
        {
            byte[] bytes = this.outputEncoding.GetBytes(this.Format(logEntry));
            this.outputStream.Write(bytes, 0, bytes.Length);

            return true;
        }
    }
}
