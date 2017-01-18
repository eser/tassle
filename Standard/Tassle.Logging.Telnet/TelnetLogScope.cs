// --------------------------------------------------------------------------
// <copyright file="TelnetLogScope.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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

#if NET451
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif

namespace Tassle.Logging.Telnet
{
    public class TelnetLogScope
    {
        private readonly string _name;
        private readonly object _state;

        internal TelnetLogScope(string name, object state) {
            this._name = name;
            this._state = state;
        }

        public TelnetLogScope Parent { get; private set; }

#if NET451
        private static readonly string FieldKey = $"{typeof(ConsoleLogScope).FullName}.Value.{AppDomain.CurrentDomain.Id}";

        public static ConsoleLogScope Current {
            get {
                var handle = CallContext.LogicalGetData(FieldKey) as ObjectHandle;
                if (handle == null) {
                    return default(ConsoleLogScope);
                }

                return (ConsoleLogScope)handle.Unwrap();
            }
            set {
                CallContext.LogicalSetData(FieldKey, new ObjectHandle(value));
            }
        }
#else
        private static AsyncLocal<TelnetLogScope> _value = new AsyncLocal<TelnetLogScope>();

        public static TelnetLogScope Current
        {
            set {
                _value.Value = value;
            }
            get {
                return _value.Value;
            }
        }
#endif

        public static IDisposable Push(string name, object state) {
            var temp = TelnetLogScope.Current;

            TelnetLogScope.Current = new TelnetLogScope(name, state) {
                Parent = temp
            };

            return new DisposableScope();
        }

        public override string ToString() {
            return this._state?.ToString();
        }

        private class DisposableScope : IDisposable
        {
            public void Dispose() {
                Current = Current.Parent;
            }
        }
    }
}
