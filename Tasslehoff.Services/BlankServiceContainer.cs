// --------------------------------------------------------------------------
// <copyright file="BlankServiceContainer.cs" company="-">
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

using System.Threading.Tasks;
namespace Tasslehoff.Services
{
    /// <summary>
    /// BlankServiceContainer class.
    /// </summary>
    public sealed class BlankServiceContainer : ServiceContainer
    {
        // fields

        /// <summary>
        /// Name
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Description
        /// </summary>
        private readonly string description;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlankServiceContainer"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        public BlankServiceContainer(string name, string description = "")
            : base()
        {
            this.name = name;
            this.description = description;
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return this.description;
            }
        }

        // methods

        /// <summary>
        /// 
        /// </summary>
        protected override void ServiceStart()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ServiceStop()
        {
            
        }
    }
}
