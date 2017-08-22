// --------------------------------------------------------------------------
// <copyright file="ValidationPropertyRuleBuilder{T,TProperty}.cs" company="-">
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
    public class ValidationPropertyRuleBuilder<T, TProperty> : IValidationPropertyRuleBuilder<T, TProperty> {
        private readonly IValidationRule<T> rule;
        private readonly Expression<Func<T, TProperty>> navigation;

        private RuleSet<T> conditionRuleSet;
        private RuleSet<TProperty> validationRuleSet;
        private List<string> validationDescriptions;
        private string message;

        public ValidationPropertyRuleBuilder(IValidationRule<T> rule, Expression<Func<T, TProperty>> navigation) {
            this.rule = rule;
            this.navigation = navigation;

            this.conditionRuleSet = new RuleSet<T>();
            this.validationRuleSet = new RuleSet<TProperty>();
            this.validationDescriptions = new List<string>();

            this.rule.Check = this.Check;
        }

        public IValidationPropertyRuleBuilder<T, TProperty> When(Expression<Func<T, bool>> expression) {
            this.conditionRuleSet.Add(expression);

            return this;
        }

        public IValidationPropertyRuleBuilder<T, TProperty> Must(Expression<Func<TProperty, bool>> expression, string description = null) {
            this.validationRuleSet.Add(expression);

            if (description != null) {
                this.validationDescriptions.Add(description);
            }

            return this;
        }

        public IValidationPropertyRuleBuilder<T, TProperty> WithMessage(string message) {
            this.message = message;

            return this;
        }

        private string GetNavigationPropertyName() {
            var expression = this.navigation;
            var body = expression.Body as MemberExpression;

            if (body == null) {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        private string GetMessage() {
            if (this.message != null) {
                return this.message;
            }

            if (this.validationDescriptions.Count > 0) {
                var property = this.GetNavigationPropertyName();
                var descriptions = string.Join(", ", this.validationDescriptions);

                return $"{property} should be {descriptions}";
            }

            return null;
        }

        private IValidationResult Check(T target) {
            var conditionMethod = this.conditionRuleSet.Compile();

            if (conditionMethod != null && !conditionMethod(target)) {
                return new ValidationResult() {
                    IsValid = true,
                };
            }

            var validationMethod = this.validationRuleSet.Compile();

            if (validationMethod != null) {
                var navigation = this.navigation.Compile();
                var navigatedProperty = navigation(target);

                if (!validationMethod(navigatedProperty)) {
                    return new ValidationResult() {
                        IsValid = false,
                        Errors = new List<IValidationError>() {
                            new ValidationError() {
                                Message = this.GetMessage(),
                            },
                        },
                    };
                }
            }

            return new ValidationResult() {
                IsValid = true,
            };
        }
    }
}
