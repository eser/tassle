//
//  StreamLogger.cs
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
using System.IO;
using System.Text;

namespace Tasslehoff.Library.Logger
{
    public class StreamLogger : Logger
    {
        // fields
        private readonly Stream outputStream;
        private Encoding outputEncoding;

        // constructors
        public StreamLogger(Stream output, Encoding encoding = null) : base()
        {
            this.outputStream = output;
            this.outputEncoding = encoding ?? Encoding.Default;
        }

        // attributes
        public Stream OutputStream {
            get {
                return this.outputStream;
            }
        }

        public Encoding OutputEncoding {
            get {
                return this.outputEncoding;
            }
            set {
                this.outputEncoding = value;
            }
        }

        // methods
        protected override bool WriteLog(LogEntry logEntry)
        {
            byte[] _bytes = this.outputEncoding.GetBytes(this.Format(logEntry));
            this.outputStream.Write(_bytes, 0, _bytes.Length);

            return true;
        }
    }
}

