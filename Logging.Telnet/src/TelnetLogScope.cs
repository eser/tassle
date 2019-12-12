// --------------------------------------------------------------------------
// <copyright file="TelnetLogScope.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Threading;

namespace Tassle.Logging.Telnet {
    public class TelnetLogScope {
        // fields

        private static AsyncLocal<TelnetLogScope> value = new AsyncLocal<TelnetLogScope>();

        private readonly string name;
        private readonly object state;

        private TelnetLogScope parent;

        // constructors

        internal TelnetLogScope(string name, object state) {
            this.name = name;
            this.state = state;
        }

        internal TelnetLogScope(string name, object state, TelnetLogScope parent) : this(name, state) {
            this.parent = parent;
        }

        // properties
        public static TelnetLogScope Current {
            get => TelnetLogScope.value.Value;
        }

        public TelnetLogScope Parent {
            get => this.parent;
        }

        // methods

        public static IDisposable Push(string name, object state) {
            var parent = TelnetLogScope.Current;

            TelnetLogScope.value.Value = new TelnetLogScope(name, state, parent);

            return new DisposableScope();
        }

        public override string ToString() {
            return this.state?.ToString();
        }

        private class DisposableScope : IDisposable {
            public void Dispose() {
                TelnetLogScope.value.Value = TelnetLogScope.value.Value.Parent;
            }
        }
    }
}
