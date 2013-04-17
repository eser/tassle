// -----------------------------------------------------------------------
// <copyright file="IDataEntity.cs" company="-">
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

namespace Tasslehoff.Library.DataEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// IDataEntity interface.
    /// </summary>
    public interface IDataEntity
    {
        // methods

        /// <summary>
        /// Called when [serialize].
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        void OnSerialize(ref IDictionary<string, object> dictionary);

        /// <summary>
        /// Called when [deserialize].
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        void OnDeserialize(ref IDictionary<string, object> dictionary);
    }
}
