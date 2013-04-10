// -----------------------------------------------------------------------
// <copyright file="DataEntity.cs" company="-">
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

namespace Tasslehoff.Library.DataAccess
{
    using System;
    using System.Data.Common;

    /// <summary>
    /// DataEntity wrapper class.
    /// </summary>
    /// <typeparam name="T">IDataEntity implementation.</typeparam>
    public class DataEntity<T> where T : IDataEntity, new()
    {
        // fields

        /// <summary>
        /// The map
        /// </summary>
        private readonly DataEntityMapper map;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntity{T}"/> class.
        /// </summary>
        public DataEntity()
        {
            this.map = DataEntityMapper.ReadFromClass(typeof(T));
        }

        // attributes

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public DataEntityMapper Map
        {
            get
            {
                return this.map;
            }
        }

        // methods

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Deserialized class.</returns>
        public T GetItem(DbDataReader reader)
        {
            return this.map.GetItem<T>(reader);
        }
    }
}
