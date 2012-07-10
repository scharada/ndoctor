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
namespace Probel.NDoctor.Domain.Test.Data
{
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Macro;
    using Probel.NDoctor.Domain.Test.Helpers;

    namespace Interview.Test
    {
        [TestFixture]
        [Category(Categories.Macros)]
        public class TestMacros
        {
            [Test]
            public void CreateMacro_ReplaceFullName_MarkupsReplacedWithValue()
            {
                var robert = "Robert";
                var dupont = "Dupont";

                var macro = "Hello $firstname$ $lastname$";
                var expected = "Hello " + robert + " " + dupont;

                var patient = new Patient() { FirstName = robert, LastName = dupont };

                var builder = new MacroBuilder(patient);
                var result = builder.Resolve(macro);

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void CreateMacro_UsingUpperCaseMarkups_MarkupsReplacesWithValues()
            {
                var robert = "Robert";
                var dupont = "Dupont";

                var macro = "Hello $FIRSTNAME$ $LASTNAME$";
                var expected = "Hello " + robert + " " + dupont;

                var patient = new Patient() { FirstName = robert, LastName = dupont };

                var builder = new MacroBuilder(patient);
                var result = builder.Resolve(macro);

                Assert.AreEqual(expected, result);
            }

            [Test]
            public void ListMarkups_ListAllTheMarkups_AllIsListed()
            {
                var result = Markups.All();

                Assert.AreEqual(7, result.Length);
            }

            [Test]
            public void CreateMacro_UsingUnknownMarkups_ExceptionIsThrown()
            {
                var macro = "Hello $unknown$ $LASTNAME$";
                var patient = new Patient() { FirstName = "ROBERT", LastName = "DUPONT" };

                var builder = new MacroBuilder(patient);
                Assert.Throws<InvalidMacroException>(() => builder.Resolve(macro));
            }
        }
    }
}