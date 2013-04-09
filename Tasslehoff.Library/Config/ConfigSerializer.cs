//
//  ConfigSerializer.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Tasslehoff.Library.Config
{
    public static class ConfigSerializer
    {
        // methods
        public static XmlObjectSerializer GetSerializer(Type type) {
            /*
            DataContractJsonSerializerSettings _settings = new DataContractJsonSerializerSettings() {
                UseSimpleDictionaryFormat = true,
                SerializeReadOnlyTypes = false
            };
            */

            return new DataContractJsonSerializer(type);
        }

        public static T Load<T>(Stream input, bool resetFirst = true) {
            XmlObjectSerializer _serializer = ConfigSerializer.GetSerializer(typeof(T));
            if(resetFirst) {
                input.Position = 0;
            }
            T _return = (T)_serializer.ReadObject(input);

            return _return;
        }

        public static void Save(Stream output, IConfig configObject, bool flushAfter = true) {
            Type _type = configObject.GetType();
            
            XmlObjectSerializer _serializer = ConfigSerializer.GetSerializer(_type);
            _serializer.WriteObject(output, configObject);

            if(flushAfter) {
                output.Flush();
            }
        }

        public static void Reset(IConfig configObject) {
            Type _type = configObject.GetType();
            
            foreach(PropertyInfo _property in _type.GetProperties()) {
                object _value = null;
                ConfigEntryAttribute[] _attributes = (ConfigEntryAttribute[])_property.GetCustomAttributes(typeof(ConfigEntryAttribute), true);
                
                if(_attributes.Length > 0) {
                    if(_attributes[0].SkipInReset) {
                        continue;
                    }
                    
                    _value = _attributes[0].DefaultValue;
                }
                
                _property.SetValue(configObject, _value, null);
            }
        }
    }
}

