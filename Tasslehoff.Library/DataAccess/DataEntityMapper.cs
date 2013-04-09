//
//  DataEntityMapper.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Tasslehoff.Library.Collections;

namespace Tasslehoff.Library.DataAccess
{
    public class DataEntityMapper : DictionaryBase<DataEntityFieldAttribute>
    {
        public DataEntityMapper() : base()
        {
        }

        public static DataEntityMapper ReadFromClass(Type type) {
            DataEntityMapper _mappings = new DataEntityMapper();
            
            MemberInfo[] _members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach(MemberInfo _member in _members) {
                if(_member.MemberType != MemberTypes.Field && _member.MemberType != MemberTypes.Property) {
                    continue;
                }
                
                object[] _attributes = _member.GetCustomAttributes(typeof(DataEntityFieldAttribute), true);
                foreach(object _attribute in _attributes) {
                    DataEntityFieldAttribute _fieldAttribute = (DataEntityFieldAttribute)_attribute;
                    
                    if(string.IsNullOrEmpty(_fieldAttribute.FieldName)) {
                        _fieldAttribute.FieldName = _member.Name;
                    }
                    
                    _fieldAttribute.ClassMember = _member;
                    if(_fieldAttribute.ClassMember.MemberType == MemberTypes.Field) {
                        _fieldAttribute.Type = (_member as FieldInfo).FieldType;
                    }
                    else if(_fieldAttribute.ClassMember.MemberType == MemberTypes.Property) {
                        _fieldAttribute.Type = (_member as PropertyInfo).PropertyType;
                    }
                    
                    if(_fieldAttribute.UnderlyingType == null) {
                        _fieldAttribute.UnderlyingType = _fieldAttribute.Type;
                    }

                    _mappings.Add(_fieldAttribute.FieldName, _fieldAttribute);
                }
            }
            
            return _mappings;
        }

        public static IDictionary<string, object> GetItem(IDataReader reader) {
            IDictionary<string, object> _dictionary = new Dictionary<string, object>();
            
            for(int _i = 0;_i < reader.FieldCount;_i++) {
                string _fieldName = reader.GetName(_i);
                
                object _fieldValue = reader.GetValue(_i);
                if(Convert.IsDBNull(_fieldValue)) {
                    _fieldValue = null;
                }
                
                _dictionary.Add(_fieldName, _fieldValue);
            }
            
            return _dictionary;
        }

        public T GetItem<T>(IDataReader reader) where T : new() {
            T _instance = new T();
            
            for(int _i = 0;_i < reader.FieldCount;_i++) {
                string _fieldName = reader.GetName(_i);
                
                if(!this.ContainsKey(_fieldName)) {
                    continue;
                }
                
                object _fieldValue = reader.GetValue(_i);
                if(Convert.IsDBNull(_fieldValue)) {
                    _fieldValue = null;
                }
                
                if(this[_fieldName].GetFunction != null) {
                    _fieldValue = this[_fieldName].GetFunction(_fieldValue);
                }
                
                DataEntityMapper.WriteTypeValue(this[_fieldName], _instance, _fieldValue);
            }
            
            return _instance;
        }

        public static object ReadTypeValue(DataEntityFieldAttribute field, object instance) {
            object _value = null;
            if(field.ClassMember.MemberType == MemberTypes.Field) {
                _value = (field.ClassMember as FieldInfo).GetValue(instance);
            }
            else if(field.ClassMember.MemberType == MemberTypes.Property) {
                _value = (field.ClassMember as PropertyInfo).GetValue(instance, null);
            }
            
            return _value;
        }
        
        public static void WriteTypeValue(DataEntityFieldAttribute field, object instance, object value) {
            if(field.ClassMember.MemberType == MemberTypes.Field) {
                (field.ClassMember as FieldInfo).SetValue(instance, value);
            }
            else if(field.ClassMember.MemberType == MemberTypes.Property) {
                (field.ClassMember as PropertyInfo).SetValue(instance, value, null);
            }
        }
    }
}

