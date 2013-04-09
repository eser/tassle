//
//  ConfigEntryAttribute.cs
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

namespace Tasslehoff.Library.Config
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigEntryAttribute : Attribute
    {
        // fields
        private object defaultValue;
        private bool isCrypted;
        private bool skipInReset;

        // constructors
        public ConfigEntryAttribute() : base()
        {
        }

        // attributes
        public object DefaultValue {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

        public bool IsCrypted {
            get { return this.isCrypted; }
            set { this.isCrypted = value; }
        }

        public bool SkipInReset {
            get { return this.skipInReset; }
            set { this.skipInReset = value; }
        }
    }
}

