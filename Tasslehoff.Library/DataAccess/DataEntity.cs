//
//  DataEntity.cs
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
using System.Data.Common;

namespace Tasslehoff.Library.DataAccess
{
    public class DataEntity<T> where T : class, new()
    {
        private readonly DataEntityMapper map;

        public DataEntity()
        {
            this.map = DataEntityMapper.ReadFromClass(typeof(T));
        }

        public DataEntityMapper Map {
            get {
                return this.map;
            }
        }

        public T GetItem(DbDataReader reader) {
            return this.map.GetItem<T>(reader);
        }
    }
}

