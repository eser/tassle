// --------------------------------------------------------------------------
// <copyright file="FileLogger.cs" company="-">
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

using System.IO;
using System.Text;

namespace Tasslehoff.Logging
{
    /// <summary>
    /// FileLogger class.
    /// </summary>
    public class FileLogger : Logger
    {
        // fields

        /// <summary>
        /// The output path
        /// </summary>
        private readonly string outputPath;

        /// <summary>
        /// The output encoding
        /// </summary>
        private Encoding outputEncoding;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="encoding">The encoding</param>
        public FileLogger(string path, Encoding encoding = null) : base()
        {
            this.outputPath = path;
            this.outputEncoding = encoding ?? Encoding.Default;
        }

        // properties

        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        public string OutputPath
        {
            get
            {
                return this.outputPath;
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
            File.AppendAllText(this.outputPath, this.Format(logEntry), this.outputEncoding);

            return true;
        }
    }
}
