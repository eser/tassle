// --------------------------------------------------------------------------
// <copyright file="LayoutBody.cs" company="-">
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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Tasslehoff.Layout.LayoutControls
{
    /// <summary>
    /// LayoutBody class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutItem(DisplayName = "Layout Body", Icon = "th")]
    public class LayoutBody : LayoutControl
    {
        // fields

        /// <summary>
        /// The title
        /// </summary>
        [DataMember]
        private string title;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBody"/> class.
        /// </summary>
        public LayoutBody()
            : base()
        {
        }

        // properties

        [DataMember]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        // methods

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <returns>
        /// HTML
        /// </returns>
        public override string Render(Controller controller)
        {
            return this.RenderChildren(controller);
        }

        /// <summary>
        /// Occurs when [get properties].
        /// </summary>
        /// <param name="properties">List of properties</param>
        public override void OnGetProperties(List<LayoutControlProperty> properties)
        {
            properties.Add(new LayoutControlProperty("Title", "Title", this.Title));
        }
    }
}
