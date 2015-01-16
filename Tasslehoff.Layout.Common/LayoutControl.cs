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
using System.Linq;
using System.Runtime.Serialization;
using Tasslehoff.Common.Text;

namespace Tasslehoff.Layout.Common
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
        /// Parameters
        /// </summary>
        [IgnoreDataMember]
        private Dictionary<string, object> parameters;

        /// <summary>
        /// Child objects
        /// </summary>
        [DataMember(Name = "Children")]
        private IList<ILayoutControl> children;

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        private string id;

        /// <summary>
        /// Static client id
        /// </summary>
        [DataMember(Name = "StaticClientId")]
        private bool staticClientId;

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

        /// <summary>
        /// The webcontrol
        /// </summary>
        [NonSerialized]
        [IgnoreDataMember]
        private object webControl;

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
        /// Gets or sets parameters
        /// </summary>
        /// <value>
        /// Parameters
        /// </value>
        [IgnoreDataMember]
        public virtual Dictionary<string, object> Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
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
        /// Gets or sets static client id
        /// </summary>
        /// <value>
        /// Static client id
        /// </value>
        [IgnoreDataMember]
        public virtual bool StaticClientId
        {
            get
            {
                return this.staticClientId;
            }
            set
            {
                this.staticClientId = value;
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


        /// <summary>
        /// Gets or sets webcontrol
        /// </summary>
        /// <value>
        /// Webcontrol
        /// </value>
        [IgnoreDataMember]
        public object WebControl
        {
            get
            {
                return this.webControl;
            }
            set
            {
                this.webControl = value;
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
        /// Initializes the layout control
        /// </summary>
        public virtual void Init()
        {
            this.Init(new Dictionary<string, object>());
        }

        /// <summary>
        /// Initializes the layout control
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public virtual void Init(Dictionary<string, object> parameters)
        {
            this.Parameters = parameters;

            foreach (ILayoutControl layoutControl in this.Children)
            {
                layoutControl.Init(parameters);
            }

            this.OnInit(parameters);
        }

        /// <summary>
        /// Occurs when [init].
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public abstract void OnInit(Dictionary<string, object> parameters);

        /// <summary>
        /// Creates web control
        /// </summary>
        public abstract void CreateWebControl();

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

            if (!string.IsNullOrEmpty(this.Id))
            {
                jsonOutputWriter.WriteProperty("Id", this.Id);
            }
            if (this.StaticClientId != false)
            {
                jsonOutputWriter.WriteProperty("StaticClientId", this.StaticClientId);
            }

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

            this.OnExport(jsonOutputWriter);

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
        public abstract void OnExport(MultiFormatOutputWriter jsonOutputWriter);

        /// <summary>
        /// Gets editable properties
        /// </summary>
        /// <returns>List of properties</returns>
        public virtual Dictionary<string, string> GetEditProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Id", "Id");
            properties.Add("StaticClientId", "Static Client Id");
            properties.Add("CssClass", "Css Class");
            properties.Add("Span", "Span");
            properties.Add("Offset", "Offset");

            this.OnGetEditProperties(properties);

            return properties;
        }

        /// <summary>
        /// Occurs when [get edit properties].
        /// </summary>
        /// <param name="properties">List of properties</param>
        public abstract void OnGetEditProperties(Dictionary<string, string> properties);

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
