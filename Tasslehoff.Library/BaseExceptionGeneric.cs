// -----------------------------------------------------------------------
// <copyright file="BaseExceptionGeneric.cs" company="-">
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

namespace Tasslehoff.Library
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// BaseExceptionGeneric class.
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    /// <remarks>You can use BaseException&lt;dynamic&gt; to use dynamic parameters.</remarks>
    [Serializable]
    public class BaseException<T> : BaseException
    {
        // fields

        /// <summary>
        /// The exception object
        /// </summary>
        private T exceptionObject;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        public BaseException(T exceptionObject) : base()
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BaseException(T exceptionObject, string message) : base(message)
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public BaseException(T exceptionObject, string message, Exception innerException) : base(message, innerException)
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        // properties

        /// <summary>
        /// Gets the exception object.
        /// </summary>
        /// <value>
        /// The exception object.
        /// </value>
        public T ExceptionObject
        {
            get
            {
                return this.exceptionObject;
            }
        }
    }
}
