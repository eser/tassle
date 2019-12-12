// --------------------------------------------------------------------------
// <copyright file="ValidationPropertyRuleBuilder{T,TProperty}.cs" company="-">
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

        private ValidationError GetError() {
            var valError = new ValidationError() {
                FieldName = this.GetNavigationPropertyName()
            };

            if (this.message != null) {
                valError.Message = this.message;
                return valError;
            }

            if (this.validationDescriptions.Count > 0) {
                var descriptions = string.Join(", ", this.validationDescriptions);
                valError.Message = $"{valError.FieldName} should be {descriptions}";

                return valError;
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
                        Errors = new List<IValidationError>() { this.GetError() },
                    };
                };
            }

            return new ValidationResult() {
                IsValid = true,
            };
        }
    }
}
