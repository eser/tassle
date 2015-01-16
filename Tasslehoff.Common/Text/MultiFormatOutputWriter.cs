// --------------------------------------------------------------------------
// <copyright file="MultiFormatOutputWriter.cs" company="-">
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

namespace Tasslehoff.Common.Text
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using Tasslehoff.Common.Helpers;
    using Xml = System.Xml;

    /// <summary>
    /// MultiFormatOutputWriter class.
    /// </summary>
    public class MultiFormatOutputWriter : IDisposable
    {
        // fields

        /// <summary>
        /// Format
        /// </summary>
        private readonly MultiFormatOutputWriterFormat format;

        /// <summary>
        /// StringBuilder
        /// </summary>
        private StringBuilder stringBuilder;

        /// <summary>
        /// TextWriter
        /// </summary>
        private TextWriter textWriter;

        /// <summary>
        /// Json TextWriter
        /// </summary>
        private JsonTextWriter jsonTextWriter;

        /// <summary>
        /// Xml TextWriter
        /// </summary>
        private Xml.XmlTextWriter xmlTextWriter;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiFormatOutputWriter"/> class.
        /// </summary>
        /// <param name="format">Format</param>
        public MultiFormatOutputWriter(MultiFormatOutputWriterFormat format)
            : this(format, new StringBuilder())
        {
            this.format = format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiFormatOutputWriter"/> class.
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="stringBuilder">The string builder</param>
        public MultiFormatOutputWriter(MultiFormatOutputWriterFormat format, StringBuilder stringBuilder)
            : base()
        {
            this.stringBuilder = stringBuilder;
            this.textWriter = new StringWriter(this.stringBuilder);
            this.jsonTextWriter = new JsonTextWriter(this.textWriter)
            {
                Formatting = Formatting.Indented
            };
            this.xmlTextWriter = new Xml.XmlTextWriter(this.textWriter)
            {
                Formatting = Xml.Formatting.Indented
            };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MultiFormatOutputWriter"/> class.
        /// </summary>
        ~MultiFormatOutputWriter()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets or Sets Format
        /// </summary>
        public MultiFormatOutputWriterFormat Format
        {
            get
            {
                return this.format;
            }
        }

        /// <summary>
        /// Gets or Sets StringBuilder
        /// </summary>
        public StringBuilder StringBuilder
        {
            get
            {
                return this.stringBuilder;
            }
            set
            {
                this.stringBuilder = value;
            }
        }

        /// <summary>
        /// Gets or Sets TextWriter
        /// </summary>
        public TextWriter TextWriter
        {
            get
            {
                return this.textWriter;
            }
            set
            {
                this.textWriter = value;
            }
        }

        /// <summary>
        /// Gets or Sets Json TextWriter
        /// </summary>
        internal JsonTextWriter JsonTextWriter
        {
            get
            {
                return this.jsonTextWriter;
            }
            set
            {
                this.jsonTextWriter = value;
            }
        }

        /// <summary>
        /// Gets or Sets Xml TextWriter
        /// </summary>
        internal Xml.XmlTextWriter XmlTextWriter
        {
            get
            {
                return this.xmlTextWriter;
            }
            set
            {
                this.xmlTextWriter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MultiFormatOutputWriter"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }
            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Writes the end of current Json tag
        /// </summary>
        public void WriteEnd()
        {
            this.JsonTextWriter.WriteEnd();
        }

        /// <summary>
        /// Writes raw Json
        /// </summary>
        /// <param name="rawString">Raw Json</param>
        public void WriteRaw(string rawString)
        {
            if (this.Format == MultiFormatOutputWriterFormat.Json)
            {
                this.JsonTextWriter.WriteRaw(rawString);
            }
            else
            {
                this.XmlTextWriter.WriteRaw(rawString);
            }
        }

        /// <summary>
        /// Writes the beginning of a Json array
        /// </summary>
        public void WriteStartArray()
        {
            this.JsonTextWriter.WriteStartArray();
        }

        /// <summary>
        /// Writes the beginning of a Json object
        /// </summary>
        public void WriteStartObject()
        {
            this.JsonTextWriter.WriteStartObject();
        }

        /// <summary>
        /// Writes a complete Json property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <param name="escape">Whether escaping is enabled or not</param>
        public void WriteProperty(string name, object value, bool escape = true)
        {
            this.WritePropertyName(name, escape);
            this.WriteValue(value);
        }

        /// <summary>
        /// Writes a complete raw Json property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <param name="escape">Whether escaping is enabled or not</param>
        public void WritePropertyRaw(string name, string value, bool escape = true)
        {
            this.WritePropertyName(name, escape);

            int indentation = this.JsonTextWriter.Indentation;

            string[] splitted = value.Split('\n');
            StringBuilder newValue = new StringBuilder();
            if (splitted.Length > 1)
            {
                newValue.AppendLine();
            }
            foreach (string line in splitted)
            {
                newValue.Append(new string(this.JsonTextWriter.IndentChar, indentation));
                newValue.AppendLine(line.TrimEnd());
            }

            this.WriteValueRaw(newValue.ToString().TrimEnd());
        }

        /// <summary>
        /// Writes the name of a Json property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="escape">Whether escaping is enabled or not</param>
        public void WritePropertyName(string name, bool escape = true)
        {
            this.JsonTextWriter.WritePropertyName(name, escape);
        }

        /// <summary>
        /// Writes the value of a Json property
        /// </summary>
        /// <param name="value">Value of the property</param>
        public void WriteValue(object value)
        {
            if (value is MultiFormatOutputWriterPropertyValue)
            {
                switch ((MultiFormatOutputWriterPropertyValue)value)
                {
                    case MultiFormatOutputWriterPropertyValue.Undefined:
                        this.JsonTextWriter.WriteUndefined();
                        break;

                    case MultiFormatOutputWriterPropertyValue.Null:
                        this.JsonTextWriter.WriteNull();
                        break;
                }

                return;
            }

            if (value != null && value.GetType().IsEnum)
            {
                this.JsonTextWriter.WriteValue(value.ToString());
                return;
            }

            this.JsonTextWriter.WriteValue(value);
        }

        /// <summary>
        /// Writes the raw Json for a value field
        /// </summary>
        /// <param name="value">Value in raw Json</param>
        public void WriteValueRaw(string value)
        {
            this.JsonTextWriter.WriteRawValue(value);
        }

        /// <summary>
        /// Writes the new line
        /// </summary>
        public void WriteLine()
        {
            // this.JsonTextWriter.WriteRaw(Environment.NewLine);
        }

        /// <summary>
        /// Converts the object instance to serialized Json string
        /// </summary>
        /// <returns>Json string</returns>
        public override string ToString()
        {
            return this.StringBuilder.ToString();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            if (releaseManagedResources)
            {
                // this.JsonTextWriter.Close();
                this.TextWriter.Dispose();
            }

            VariableHelpers.CheckAndDispose<TextWriter>(ref this.textWriter);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "textWriter")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }
    }
}