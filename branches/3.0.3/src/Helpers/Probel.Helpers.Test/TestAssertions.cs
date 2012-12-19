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

    public class TestAssertion : TestBase
    {
        #region Methods

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void AssertBoolean_SpecifyInvalidData_ThrowsAsertionException()
        {
            Probel.Helpers.Assertion.Assert.IsFalse(true);
        }

        [Test]
        public void AssertBoolean_SpecifyValidData_Valid()
        {
            ContractAssert.IsFalse(false);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void AssertIsNotNull_SpecifyInvalidData_ThrowsAsertionException()
        {
            ContractAssert.IsNull(new object());
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void AssertIsNotNull_SpecifyNull_ThrowsAsertionException()
        {
            ContractAssert.IsNotNull(null);
        }

        [Test]
        public void AssertIsNotNull_SpecifyValidData_Valid()
        {
            ContractAssert.IsNotNull(new object());
        }

        [Test]
        public void AssertIsNull_SpecifyValidData_Valid()
        {
            ContractAssert.IsNull(null);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void AssertIsTrue_SpecifyInvalidData_ThrowsAsertionException()
        {
            ContractAssert.IsTrue(false);
        }

        [Test]
        public void AssertIsTrue_SpecifyValidData_Valid()
        {
            ContractAssert.IsTrue(true);
        }

        [ExpectedException(typeof(ContractException))]
        [Test]
        public void AssertOfType_SpecifyInt_Valid()
        {
            Probel.Helpers.Assertion.Assert.OfType<DateTime>(4);
        }

        [Test]
        public void AssertOfType_SpecifyNow_Valid()
        {
            Probel.Helpers.Assertion.Assert.OfType<DateTime>(DateTime.Now);
        }

        [Test]
        public void AssertsValues_SpecifyInvalidData_ThrowsAsertionException()
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

        #endregion Methods
    }
}