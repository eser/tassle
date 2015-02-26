// --------------------------------------------------------------------------
// <copyright file="LayoutControlRegistry.cs" company="-">
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
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Tasslehoff.Common.Helpers;
using Tasslehoff.DataStructures.Collections;

namespace Tasslehoff.Layout
{
    /// <summary>
    /// LayoutControlRegistry class.
    /// </summary>
    [ComVisible(false)]
    public class LayoutControlRegistry : DictionaryBase<string, LayoutPropertiesAttribute>
    {
        // fields

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static LayoutControlRegistry instance = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControlRegistry"/> class.
        /// </summary>
        public LayoutControlRegistry() : base()
        {
            if (LayoutControlRegistry.instance == null)
            {
                LayoutControlRegistry.instance = this;
            }
        }

        // properties

        /// <summary>
        /// Gets or Sets singleton instance.
        /// </summary>
        public static LayoutControlRegistry Instance
        {
            get
            {
                return LayoutControlRegistry.instance;
            }
            set
            {
                LayoutControlRegistry.instance = value;
            }
        }

        // methods

        /// <summary>
        /// Registers a data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        public void Register<T>() where T : ILayoutControl, new()
        {
            Type type = typeof(T);

            object[] attributes = type.GetCustomAttributes(typeof(LayoutPropertiesAttribute), true);
            foreach (object attribute in attributes)
            {
                LayoutPropertiesAttribute typeAttribute = (LayoutPropertiesAttribute)attribute;

                typeAttribute.Type = type;

                this.Add(type.Name, typeAttribute);
                break;
            }
        }

        /// <summary>
        /// Creates a new instance of related type.
        /// </summary>
        /// <param name="key">Key of the element</param>
        /// <returns>Created instance</returns>
        public ILayoutControl Create(string key)
        {
            LayoutPropertiesAttribute layoutProperties = this[key];

            ILayoutControl instance = Activator.CreateInstance(layoutProperties.Type) as ILayoutControl;
            return instance;
        }

        /// <summary>
        /// Imports Json
        /// </summary>
        /// <param name="json">Json string</param>
        /// <returns>ILayoutControl implementation</returns>
        public ILayoutControl ImportJson(string json)
        {
            JsonSerializerSettings settings = SerializationHelpers.GetSerializerSettings();
            settings.Converters.Add(new LayoutControlConverter(this));

            //JObject jObject = JObject.Parse(json);
            //Type type = this[(string)jObject.Property("Type")];

            //return (ILayoutControl)JsonConvert.DeserializeObject(json, type, settings);
            return JsonConvert.DeserializeObject<ILayoutControl>(json, settings);
        }
    }
}
