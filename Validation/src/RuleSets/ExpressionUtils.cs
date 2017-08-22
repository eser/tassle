// --------------------------------------------------------------------------
// <copyright file="ExpressionUtils.cs" company="-">
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
