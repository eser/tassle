// --------------------------------------------------------------------------
// <copyright file="HtmlTag.cs" company="-">
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
using System.Text;
using System.Web.Mvc;
using Tasslehoff.Common.Text;

namespace Tasslehoff.Layout.LayoutControls
{
    /// <summary>
    /// HtmlTag class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutProperties(DisplayName = "Html Tag", Icon = "th")]
    public class HtmlTag : LayoutControl
    {
        // fields

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        private string id;

        /// <summary>
        /// TagName
        /// </summary>
        [DataMember(Name = "TagName")]
        private string tagName;

        /// <summary>
        /// Css Class
        /// </summary>
        [DataMember(Name = "CssClass")]
        private string cssClass;

        /// <summary>
        /// Span
        /// </summary>
        [DataMember(Name = "Span")]
        private int span;

        /// <summary>
        /// Offset
        /// </summary>
        [DataMember(Name = "Offset")]
        private int offset;
        
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlTag"/> class.
        /// </summary>
        public HtmlTag()
            : base()
        {
        }

        // properties

        /// <summary>
        /// Gets or sets id
        /// </summary>
        /// <value>
        /// Id
        /// </value>
        [IgnoreDataMember]
        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        /// <value>
        /// TagName
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
        /// Gets or sets css class
        /// </summary>
        /// <value>
        /// Css class
        /// </value>
        [IgnoreDataMember]
        public virtual string CssClass
        {
            get
            {
                return this.cssClass;
            }
            set
            {
                this.cssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets span
        /// </summary>
        /// <value>
        /// Span
        /// </value>
        [IgnoreDataMember]
        public virtual int Span
        {
            get
            {
                return this.span;
            }
            set
            {
                this.span = value;
            }
        }

        /// <summary>
        /// Gets or sets offset
        /// </summary>
        /// <value>
        /// Offset
        /// </value>
        [IgnoreDataMember]
        public virtual int Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
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
            TagBuilder tagBuilder = new TagBuilder(this.TagName);

            if (!string.IsNullOrEmpty(this.Id))
            {
                tagBuilder.MergeAttribute("id", this.Id, true);
            }

            string classNames = this.ConstructClassAttribute();
            if (!string.IsNullOrEmpty(classNames))
            {
                tagBuilder.MergeAttribute("class", classNames, true);
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (ILayoutControl control in this.Children)
            {
                string output = control.Render(controller);
                if (!string.IsNullOrEmpty(output))
                {
                    stringBuilder.Append(output);
                }
            }

            tagBuilder.InnerHtml = stringBuilder.ToString();

            return tagBuilder.ToString();
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
            if (!string.IsNullOrEmpty(this.Id))
            {
                jsonOutputWriter.WriteProperty("Id", this.Id);
            }

            jsonOutputWriter.WriteProperty("TagName", this.TagName);

            jsonOutputWriter.WriteLine();

            if (!string.IsNullOrEmpty(this.CssClass))
            {
                jsonOutputWriter.WriteProperty("CssClass", this.CssClass);
            }

            if (this.Span != 0)
            {
                jsonOutputWriter.WriteProperty("Span", this.Span);
            }

            if (this.Offset != 0)
            {
                jsonOutputWriter.WriteProperty("Offset", this.Offset);
            }
        }

        /// <summary>
        /// Occurs when [get edit properties].
        /// </summary>
        /// <param name="properties">List of properties</param>
        public override void OnGetEditProperties(Dictionary<string, string> properties)
        {
            properties.Add("Id", "Id");
            properties.Add("TagName", "Tag Name");
            properties.Add("CssClass", "Css Class");
            properties.Add("Span", "Span");
            properties.Add("Offset", "Offset");
        }

        /// <summary>
        /// Constructs class attribute value for HTML selement
        /// </summary>
        /// <returns>Class names</returns>
        protected virtual string ConstructClassAttribute()
        {
            string classNames = this.CssClass ?? string.Empty;

            if (this.Span != 0)
            {
                if (classNames.Length > 0)
                {
                    classNames += " ";
                }

                classNames += "col-xs-" + this.Span;
            }

            if (this.Offset != 0)
            {
                if (classNames.Length > 0)
                {
                    classNames += " ";
                }

                classNames += "col-xs-offset-" + this.Offset;
            }

            return classNames;
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources)
        {
            // VariableUtils.CheckAndDispose<LoggerDelegate>(ref this.log);
        }
    }
}
