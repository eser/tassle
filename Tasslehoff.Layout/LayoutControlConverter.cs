// --------------------------------------------------------------------------
// <copyright file="LayoutControlConverter.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Tasslehoff.Layout
{
    /// <summary>
    /// LayoutControlConverter class.
    /// </summary>
    [ComVisible(false)]
    public class LayoutControlConverter : CustomCreationConverter<ILayoutControl>
    {
        // fields

        /// <summary>
        /// 
        /// </summary>
        private readonly LayoutControlRegistry registry;

        // constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registry"></param>
        public LayoutControlConverter(LayoutControlRegistry registry)
            : base()
        {
            this.registry = registry;
        }

        // methods

        /// <summary>
        /// Constructs target instance in proper type
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        internal static ILayoutControl ConstructProperTarget(LayoutControlRegistry registry, JObject jObject)
        {
            // Get type from JObject
            string type = (string)jObject.Property("Type");

            // Create target object based on JObject
            return registry.Create(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override ILayoutControl Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            ILayoutControl target = LayoutControlConverter.ConstructProperTarget(this.registry, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
