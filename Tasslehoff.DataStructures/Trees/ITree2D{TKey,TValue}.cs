// --------------------------------------------------------------------------
// <copyright file="ITree2D{TKey,TValue}.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
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

using System.Collections.Generic;

namespace Tasslehoff.DataStructures.Trees
{
    /// <summary>
    /// ITree2D&lt;TKey, TValue&gt; interface.
    /// </summary>
    public interface ITree2D<TKey, TValue> : ITreeCommon<TValue>
    {
        // properties

        /// <summary>
        /// Gets or sets tree id
        /// </summary>
        /// <value>
        /// Tree Id
        /// </value>
        TKey TreeId { get; set;  }

        /// <summary>
        /// Gets or sets parent tree id
        /// </summary>
        /// <value>
        /// Parent Tree Id
        /// </value>
        TKey ParentTreeId { get; set; }

        /// <summary>
        /// Gets or sets sort index
        /// </summary>
        /// <value>
        /// Sort index
        /// </value>
        short SortIndex { get; set; }

        // methods

        /// <summary>
        /// Sets some ids to produce a tree
        /// </summary>
        /// <param name="isRoot">Whether this node is root or not</param>
        void MakeTree(bool isRoot = false);

        /// <summary>
        /// Flattens tree into one list
        /// </summary>
        /// <returns>Generated list</returns>
        List<TValue> FlattenTree();
    }
}
