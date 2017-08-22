// --------------------------------------------------------------------------
// <copyright file="IValidationPropertyRuleBuilder{T,TProperty}.cs" company="-">
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
