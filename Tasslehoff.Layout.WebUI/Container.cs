// --------------------------------------------------------------------------
// <copyright file="Container.cs" company="-">
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
using System.Web.UI.HtmlControls;
using Tasslehoff.Common.Text;
using Tasslehoff.Layout.Common;

namespace Tasslehoff.Layout.WebUI
{
    /// <summary>
    /// Container class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutProperties(DisplayName = "Container", Icon = "th-large")]
    public class Container : Base
    {
        // fields

        /// <summary>
        /// Tag name
        /// </summary>
        [DataMember(Name = "TagName")]
        private string tagName = "div";

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        private string title = string.Empty;

        // properties

        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        /// <value>
        /// Tag name
        /// </value>
        [IgnoreDataMember]
        public virtual string TagName
        {
            get
            {
                return this.tagName;
            }
            set
            {
                this.tagName = value;
            }
        }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        /// <value>
        /// Title
        /// </value>
        [IgnoreDataMember]
        public virtual string Title
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
        /// Creates web control
        /// </summary>
        public override void CreateWebControl()
        {
            HtmlGenericControl element = new HtmlGenericControl(this.TagName);

            this.AddWebControlAttributes(element, element.Attributes);
            this.AddWebControlChildren(element);
            
            this.WebControl = element;
        }

        /// <summary>
        /// Occurs when [init].
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public override void OnInit(Dictionary<string, object> parameters)
        {
            base.OnInit(parameters);
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
            base.OnExport(jsonOutputWriter);

            if (this.TagName != "div")
            {
                jsonOutputWriter.WriteProperty("TagName", this.TagName);
            }

            if (!string.IsNullOrEmpty(this.Title))
            {
                jsonOutputWriter.WriteProperty("Title", this.Title);
            }
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnGetEditProperties(Dictionary<string, string> properties)
        {
            base.OnGetEditProperties(properties);

            properties.Add("TagName", "Tag Name");
            properties.Add("Title", "Title");
        }
    }
}
