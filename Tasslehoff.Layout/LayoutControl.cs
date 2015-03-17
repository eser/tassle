// --------------------------------------------------------------------------
// <copyright file="LayoutControl.cs" company="-">
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;
using Tasslehoff.Common.Text;

namespace Tasslehoff.Layout
{
    /// <summary>
    /// LayoutControl class.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class LayoutControl : ILayoutControl
    {
        // fields

        /// <summary>
        /// Tree Id
        /// </summary>
        [IgnoreDataMember]
        private Guid treeId;

        /// <summary>
        /// Parent Tree Id
        /// </summary>
        [IgnoreDataMember]
        private Guid parentTreeId;

        /// <summary>
        /// Sort Index
        /// </summary>
        [IgnoreDataMember]
        private short sortIndex;

        /// <summary>
        /// Type
        /// </summary>
        [IgnoreDataMember]
        private readonly string type;

        /// <summary>
        /// Child objects
        /// </summary>
        [DataMember(Name = "Children")]
        private IList<ILayoutControl> children;

        /// <summary>
        /// The disposed
        /// </summary>
        [DataMember(Name = "Disposed")]
        private bool disposed;
        
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControl"/> class.
        /// </summary>
        public LayoutControl()
        {
            this.type = this.GetType().Name;
            this.children = new List<ILayoutControl>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LayoutControl"/> class.
        /// </summary>
        ~LayoutControl()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets or sets tree id
        /// </summary>
        /// <value>
        /// Tree Id
        /// </value>
        [IgnoreDataMember]
        public virtual Guid TreeId
        {
            get
            {
                return this.treeId;
            }
            set
            {
                this.treeId = value;
            }
        }

        /// <summary>
        /// Gets or sets parent tree id
        /// </summary>
        /// <value>
        /// Parent Tree Id
        /// </value>
        [IgnoreDataMember]
        public virtual Guid ParentTreeId
        {
            get
            {
                return this.parentTreeId;
            }
            set
            {
                this.parentTreeId = value;
            }
        }

        /// <summary>
        /// Gets or sets sort index
        /// </summary>
        /// <value>
        /// Sort index
        /// </value>
        [IgnoreDataMember]
        public virtual short SortIndex
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
        /// Gets type
        /// </summary>
        /// <value>
        /// Type
        /// </value>
        [IgnoreDataMember]
        public virtual string Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets description
        /// </summary>
        /// <value>
        /// Description
        /// </value>
        [IgnoreDataMember]
        public virtual string Description
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets icon
        /// </summary>
        /// <value>
        /// Icon
        /// </value>
        [IgnoreDataMember]
        public virtual string Icon
        {
            get
            {
                return "file-o";
            }
        }

        /// <summary>
        /// Gets or sets child objects
        /// </summary>
        /// <value>
        /// Child objects
        /// </value>
        [IgnoreDataMember]
        public virtual IList<ILayoutControl> Children
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Service"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        [IgnoreDataMember]
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }
            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <returns>HTML</returns>
        public virtual string Render(Controller controller)
        {
            return null;
        }

        /// <summary>
        /// Renders the children controls' contents
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>HTML</returns>
        public virtual string RenderChildren(Controller controller)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ILayoutControl control in this.Children)
            {
                string output = control.Render(controller);
                if (!string.IsNullOrEmpty(output))
                {
                    stringBuilder.Append(output);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets children objects filtered by type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Children objects</returns>
        public IEnumerable<T> GetChildrenType<T>() where T : ILayoutControl
        {
            foreach (ILayoutControl layoutControl in this.Children)
            {
                if (layoutControl is T)
                {
                    yield return (T)layoutControl;
                }
            }
        }

        /// <summary>
        /// Sets some ids to produce a tree
        /// </summary>
        /// <param name="isRoot">Whether this node is root or not</param>
        public virtual void MakeTree(bool isRoot = false)
        {
            this.TreeId = (isRoot) ? Guid.Empty : Guid.NewGuid();

            short idx = 0;
            foreach (ILayoutControl control in this.Children)
            {
                control.ParentTreeId = this.TreeId;
                control.SortIndex = idx++;
                control.MakeTree();
            }
        }

        /// <summary>
        /// Flattens tree into one list
        /// </summary>
        /// <returns>Generated list</returns>
        public virtual List<ILayoutControl> FlattenTree()
        {
            List<ILayoutControl> flattenTree = new List<ILayoutControl>();

            flattenTree.Add(this);
            foreach (ILayoutControl control in this.Children)
            {
                flattenTree.AddRange(control.FlattenTree());
            }
            this.Children.Clear();

            return flattenTree;
        }

        /// <summary>
        /// Serializes control into json
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public virtual void Export(MultiFormatOutputWriter jsonOutputWriter)
        {
            jsonOutputWriter.WriteStartObject();

            jsonOutputWriter.WriteProperty("Type", this.Type);

            // additional
            this.OnExport(jsonOutputWriter);

            // properties
            IEnumerable<LayoutControlProperty> properties = this.GetProperties();

            jsonOutputWriter.WritePropertyName("Properties");
            jsonOutputWriter.WriteStartObject();

            foreach (LayoutControlProperty property in properties)
            {
                jsonOutputWriter.WriteProperty(property.Name, property.Value);
            }

            jsonOutputWriter.WriteEnd();

            // children
            if (this.Children.Count > 0)
            {
                jsonOutputWriter.WritePropertyName("Children");

                jsonOutputWriter.WriteStartArray();
                foreach (ILayoutControl control in this.Children.OrderBy((x) => x.SortIndex))
                {
                    control.Export(jsonOutputWriter);
                }
                jsonOutputWriter.WriteEnd();
            }

            jsonOutputWriter.WriteEnd();
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public virtual void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
        }

        /// <summary>
        /// Gets properties
        /// </summary>
        /// <returns>List of properties</returns>
        public virtual IEnumerable<LayoutControlProperty> GetProperties()
        {
            List<LayoutControlProperty> properties = new List<LayoutControlProperty>();

            this.OnGetProperties(properties);

            return properties;
        }

        /// <summary>
        /// Occurs when [get properties].
        /// </summary>
        /// <param name="properties">List of properties</param>
        public abstract void OnGetProperties(List<LayoutControlProperty> properties);

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            LayoutControl clone = this.MemberwiseClone() as LayoutControl;

            clone.Children.Clear();
            foreach (ILayoutControl control in this.Children)
            {
                clone.Children.Add(control.Clone() as ILayoutControl);
            }

            return clone;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>|
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="tempData">The temp data.</param>
        /// <returns>Rendered HTML output</returns>
        protected string RenderPartialViewToString(Controller controller, string viewName = null, ViewDataDictionary viewData = null, TempDataDictionary tempData = null)
        {
            if (viewName == null)
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            if (viewData == null)
            {
                viewData = controller.ViewData;
            }

            if (tempData == null)
            {
                tempData = controller.TempData;
            }

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            // VariableUtils.CheckAndDispose<LoggerDelegate>(ref this.log);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }
    }
}
