// --------------------------------------------------------------------------
// <copyright file="RuleSet{T}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
