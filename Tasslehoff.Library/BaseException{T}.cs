// -----------------------------------------------------------------------
// <copyright file="BaseException{T}.cs" company="-">
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
    /// <remarks>
    /// You can use BaseException&lt;dynamic&gt; to use dynamic parameters.
    /// </remarks>
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
        /// Initializes a new instance of the <see cref="BaseException{T}"/> class.
        /// </summary>
        /// <param name="exceptionObject">The exception object</param>
        public BaseException(T exceptionObject) : base()
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException{T}"/> class.
        /// </summary>
        /// <param name="exceptionObject">The exception object</param>
        /// <param name="message">The message</param>
        public BaseException(T exceptionObject, string message) : base(message)
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException{T}"/> class.
        /// </summary>
        /// <param name="exceptionObject">The exception object</param>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        public BaseException(T exceptionObject, string message, Exception innerException) : base(message, innerException)
        {
            this.exceptionObject = exceptionObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException{T}"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination</param>
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

        // methods

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
