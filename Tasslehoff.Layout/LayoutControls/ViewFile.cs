// --------------------------------------------------------------------------
// <copyright file="ViewFile.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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
using System.Web.Mvc;
using Tasslehoff.Common.Text;

namespace Tasslehoff.Layout.LayoutControls
{
    /// <summary>
    /// ViewFile class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutItem(DisplayName = "View File", Icon = "th")]
    public class ViewFile : LayoutControl
    {
        // fields

        /// <summary>
        /// Path
        /// </summary>
        [DataMember(Name = "Path")]
        private string path;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewFile"/> class.
        /// </summary>
        public ViewFile()
            : base()
        {
        }

        // properties

        /// <summary>
        /// Gets or sets Path field
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [IgnoreDataMember]
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        // methods

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <returns>
        /// HTML
        /// </returns>
        public override string Render(Controller controller)
        {
            return this.RenderPartialViewToString(controller, this.Path);
        }

        /// <summary>
        /// Occurs when [get properties].
        /// </summary>
        /// <param name="properties">List of properties</param>
        public override void OnGetProperties(List<LayoutControlProperty> properties)
        {
            properties.Add(new LayoutControlProperty("Path", "Path of View File", this.Path));
        }
    }
}
