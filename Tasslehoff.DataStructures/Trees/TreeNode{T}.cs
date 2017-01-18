// --------------------------------------------------------------------------
// <copyright file="TreeNode{T}.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tasslehoff.DataStructures.Collections;

namespace Tasslehoff.DataStructures.Trees
{
    /// <summary>
    /// A node in tree data structure
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    [Serializable]
    [DataContract]
    public class TreeNode<T> : ITree3D<TreeNode<T>>, ICloneable, IComparable
    {
        // fields

        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "Value")]
        private T value;

        /// <summary>
        /// SortIndex
        /// </summary>
        [DataMember(Name = "SortIndex")]
        private short sortIndex;

        /// <summary>
        /// Children
        /// </summary>
        [DataMember(Name = "Children")]
        private IList<TreeNode<T>> children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        public TreeNode()
        {
            this.children = new SortedList<TreeNode<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value</param>
        public TreeNode(T value)
        {
            this.value = value;
            this.children = new SortedList<TreeNode<T>>();
        }

        // properties

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [IgnoreDataMember]
        public T Value {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort index
        /// </summary>
        [IgnoreDataMember]
        public short SortIndex
        {
            get
            {
                return this.sortIndex;
            }
            set
            {
                this.sortIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the children
        /// </summary>
        [IgnoreDataMember]
        public IList<TreeNode<T>> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value;
            }
        }

        // methods

        /// <summary>
        /// Adds a child to node.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="sortIndex">The sort index</param>
        /// <returns>Child object created</returns>
        public TreeNode<T> AddChild(T value, short? sortIndex = null)
        {
            TreeNode<T> node = new TreeNode<T>(value);
            if (sortIndex.HasValue)
            {
                node.sortIndex = sortIndex.Value;
            }

            this.children.Add(node);

            return node;
        }


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Does a comparision between two TreeNode instances
        /// </summary>
        /// <param name="obj">Other object to be compaired with</param>
        /// <returns>The result</returns>
        public int CompareTo(object obj)
        {
            TreeNode<T> other = obj as TreeNode<T>;
            if (other == null)
            {
                return 1;
            }

            return this.sortIndex.CompareTo(other.sortIndex);
        }
    }
}
