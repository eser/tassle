// --------------------------------------------------------------------------
// <copyright file="ExpressionUtils.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tassle.Validation {
    public static class ExpressionUtils {
        public static Expression<T> CombineExpressions<T>(IList<Expression<T>> expressions) {
            var length = expressions.Count;

            if (length == 0) {
                return null;
            }

            var combination = expressions[0];

            for (var i = 1; i < length; i++) {
                var expression = expressions[i];

                var visitor = new SwapVisitor(combination.Parameters[0], expression.Parameters[0]);

                combination = Expression.Lambda<T>(Expression.AndAlso(visitor.Visit(combination.Body), expression.Body), expression.Parameters);
            }

            return combination;
        }

        internal class SwapVisitor : ExpressionVisitor {
            private readonly Expression from;
            private readonly Expression to;

            public SwapVisitor(Expression from, Expression to) {
                this.from = from;
                this.to = to;
            }

            public override Expression Visit(Expression node) {
                if (node == this.from) {
                    return this.to;
                }

                return base.Visit(node);
            }
        }
    }
}
