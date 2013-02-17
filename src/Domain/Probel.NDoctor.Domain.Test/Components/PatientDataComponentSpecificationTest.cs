#region Header

/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion Header

namespace Probel.NDoctor.Domain.Test.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DTO.Specification;

    [TestFixture]
    class PatientDataComponentSpecificationTest
    {
        #region Methods

        [Test]
        public void BuildComplectSpecification_WithOperators_SpecificationWorks()
        {
            var trueValues = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 27 };
            var falseValues = new int[] { 16, 17, 18, 19, 20, 1, 0, -1, -2, -3, -4, -5, -6 };

            var expression = (When.Integer.GreaterThan(2) & When.Integer.LessThan(15))
                            | When.Integer.EqualTo(27);

            foreach (var item in trueValues)
            {
                Assert.IsTrue(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
            foreach (var item in falseValues)
            {
                Assert.IsFalse(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
        }

        [Test]
        public void BuildComplexSpecification_WithFluentInterface_SpecificationIsWorking()
        {
            var trueValues = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 27 };
            var falseValues = new int[] { 16, 17, 18, 19, 20, 1, 0, -1, -2, -3, -4, -5, -6 };

            var expression = When.Integer.GreaterThan(2)
                .And(When.Integer.LessThan(15))
                .Or(When.Integer.EqualTo(27));

            foreach (var item in trueValues)
            {
                Assert.IsTrue(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
            foreach (var item in falseValues)
            {
                Assert.IsFalse(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
        }

        [Test]
        public void CheckNotOperator_WithFluentInterface_SpecificationWorks()
        {
            var trueValues = new int[] { 6, 7, 8, 9, 10 };
            var falseValues = new int[] { 1, 2, 3, 5 };

            var expression = When.Integer.GreaterThan(4)
                .And(When.Integer.EqualTo(5).Not());

            foreach (var item in trueValues)
            {
                Assert.IsTrue(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
            foreach (var item in falseValues)
            {
                Assert.IsFalse(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
        }

        [Test]
        public void CheckNotOperator_WithOperator_SpecificationWorks()
        {
            var trueValues = new int[] { 6, 7, 8, 9, 10 };
            var falseValues = new int[] { 1, 2, 3, 5 };

            var expression = When.Integer.GreaterThan(4)
                         & (!When.Integer.EqualTo(5));

            foreach (var item in trueValues)
            {
                Assert.IsTrue(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
            foreach (var item in falseValues)
            {
                Assert.IsFalse(expression.IsSatisfiedBy(item), string.Format("Value '{0}' is failing", item));
            }
        }

        #endregion Methods
    }
}