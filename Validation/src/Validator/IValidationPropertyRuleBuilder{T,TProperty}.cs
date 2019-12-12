// --------------------------------------------------------------------------
// <copyright file="IValidationPropertyRuleBuilder{T,TProperty}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace Tassle.Validation {
    public interface IValidationPropertyRuleBuilder<T, TProperty> {
#pragma warning disable CA1716 // Rename virtual/interface member IValidationPropertyRuleBuilder<T, TProperty>.When(Expression<Func<T, bool>>) so that it no longer conflicts with the reserved language keyword 'When'. Using a reserved keyword as the name of a virtual/interface member makes it harder for consumers in other languages to override/implement the member.
        IValidationPropertyRuleBuilder<T, TProperty> When(Expression<Func<T, bool>> expression);
#pragma warning restore CA1716

        IValidationPropertyRuleBuilder<T, TProperty> Must(Expression<Func<TProperty, bool>> expression, string description = null);

        IValidationPropertyRuleBuilder<T, TProperty> WithMessage(string message);
    }
}
