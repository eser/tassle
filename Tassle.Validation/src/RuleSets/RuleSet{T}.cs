// --------------------------------------------------------------------------
// <copyright file="RuleSet{T}.cs" company="-">
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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tassle.Validation {
    public class RuleSet<T> : IRuleSet<T> {
        private readonly IList<Expression<Func<T, bool>>> expressions;

        private Expression<Func<T, bool>> combined;
        private Func<T, bool> compiled;

        public RuleSet() {
            this.expressions = new List<Expression<Func<T, bool>>>();
        }

        public void Add(Expression<Func<T, bool>> predicate) {
            this.combined = null;
            this.compiled = null;

            this.expressions.Add(predicate);
        }

        public Expression<Func<T, bool>> Combine() {
            if (this.combined == null) {
                this.combined = ExpressionUtils.CombineExpressions(this.expressions);
            }

            return this.combined;
        }

        public Func<T, bool> Compile() {
            var combined = this.Combine();

            if (this.compiled == null) {
                this.compiled = combined?.Compile();
            }

            return this.compiled;
        }

        public bool CheckFor(T target) {
            var compiled = this.Compile();

            // FIXME there is no expressions to be combined, so validation must be passed.
            if (compiled == null) {
                return true;
            }

            return compiled(target);
        }
    }
}
