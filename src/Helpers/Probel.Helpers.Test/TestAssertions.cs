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
namespace Probel.Helpers.Test
{
    using System;

    using NUnit.Framework;

    using ContractAssert = Probel.Helpers.Assertion.Assert;

    using ContractException = Probel.Helpers.Assertion.AssertionException;

    using NUnitAssert = NUnit.Framework.Assert;

    public class TestContract : TestBase
    {
        #region Methods

        [Test]
        public void TestFailingValues()
        {
            var catchCounter = 0;
            try { ContractAssert.IsNull(new object(), "Some message"); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }

            try { ContractAssert.IsNotNull(null, "Some message"); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }

            try { ContractAssert.IsTrue(false, "Some message"); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }

            try { ContractAssert.IsFalse(true, "Some message"); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }

            try { ContractAssert.Fail("Test error message for assertion."); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }
            try { ContractAssert.OfType<DateTime>(new object()); }
            catch (ContractException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                catchCounter++;
            }

            NUnitAssert.AreEqual(catchCounter, 6, "All the assertion should throw exception");
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void TestIsFalseConstraint_Failed()
        {
            Probel.Helpers.Assertion.Assert.IsFalse(true);
        }

        [Test]
        public void TestIsFalseConstraint_Succeed()
        {
            ContractAssert.IsFalse(false);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void TestIsNotNullConstraint_Failed()
        {
            ContractAssert.IsNotNull(null);
        }

        [Test]
        public void TestIsNotNullConstraint_Suceed()
        {
            ContractAssert.IsNotNull(new object());
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void TestIsNullConstraint_Failed()
        {
            ContractAssert.IsNull(new object());
        }

        [Test]
        public void TestIsNullConstraint_Suceed()
        {
            ContractAssert.IsNull(null);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void TestIsTrueConstraint_Failed()
        {
            ContractAssert.IsTrue(false);
        }

        [Test]
        public void TestIsTrueConstraint_Succeed()
        {
            ContractAssert.IsTrue(true);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void TestOfTypeConstraint_Failed()
        {
            Probel.Helpers.Assertion.Assert.OfType<DateTime>(4);
        }

        [Test]
        public void TestOfTypeConstraint_Succeeded()
        {
            Probel.Helpers.Assertion.Assert.OfType<DateTime>(DateTime.Now);
        }

        #endregion Methods
    }
}