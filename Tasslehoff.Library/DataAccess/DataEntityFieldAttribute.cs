//
//  DataEntityFieldAttribute.cs
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
using System.Reflection;

namespace Tasslehoff.Library.DataAccess
{
    public class DataEntityFieldAttribute : Attribute
    {
        private MemberInfo classMember;
        private string fieldName;
        private Type type;
        private Type underlyingType;
        private Func<object, object> getFunction;
        private Func<object, object> setFunction;

        public DataEntityFieldAttribute() : base()
        {
        }

        public MemberInfo ClassMember {
            get {
                return this.classMember;
            }
            set {
                this.classMember = value;
            }
        }

        public string FieldName {
            get {
                return this.fieldName;
            }
            set {
                this.fieldName = value;
            }
        }

        public Type Type {
            get {
                return this.type;
            }
            set {
                this.type = value;
            }
        }

        public Type UnderlyingType {
            get {
                return this.underlyingType;
            }
            set {
                this.underlyingType = value;
            }
        }

        public Func<object, object> GetFunction {
            get {
                return this.getFunction;
            }
            set {
                this.getFunction = value;
            }
        }

        public Func<object, object> SetFunction {
            get {
                return this.setFunction;
            }
            set {
                this.setFunction = value;
            }
        }
    }
}

