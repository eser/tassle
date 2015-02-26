// --------------------------------------------------------------------------
// <copyright file="ContentPart.cs" company="-">
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
using System.Web;
using System.Web.Mvc;
using Tasslehoff.Common.Text;

namespace Tasslehoff.Layout.LayoutControls
{
    /// <summary>
    /// ContentPart class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutProperties(DisplayName = "Content Part", Icon = "th")]
    public class ContentPart : LayoutControl
    {
        // fields

        /// <summary>
        /// Content
        /// </summary>
        [DataMember(Name = "Content")]
        private string content;

        /// <summary>
        /// HtmlEncode
        /// </summary>
        [DataMember(Name = "HtmlEncode")]
        private bool htmlEncode;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPart"/> class.
        /// </summary>
        public ContentPart()
            : base()
        {
        }

        // properties

        /// <summary>
        /// Gets or sets Content field
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [IgnoreDataMember]
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [HTML encode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [HTML encode]; otherwise, <c>false</c>.
        /// </value>
        [IgnoreDataMember]
        public bool HtmlEncode
        {
            get
            {
                return this.htmlEncode;
            }
            set
            {
                this.htmlEncode = value;
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
            if (this.HtmlEncode)
            {
                return HttpUtility.HtmlEncode(this.Content);
            }

            return this.Content;
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
            jsonOutputWriter.WriteProperty("Content", this.Content);
            jsonOutputWriter.WriteProperty("HtmlEncode", this.HtmlEncode);
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnGetEditProperties(Dictionary<string, string> properties)
        {
            properties.Add("Content", "Content");
            properties.Add("HtmlEncode", "Html Encode");
        }
    }
}
