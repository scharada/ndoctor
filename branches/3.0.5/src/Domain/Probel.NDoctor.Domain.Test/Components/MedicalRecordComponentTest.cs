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
    using System.Linq;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Macro;
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class MedicalRecordComponentTest : BaseComponentTest<MedicalRecordComponent>
    {
        #region Methods

        [Test]
        public void CreateMacro_WithInvalidExpression_ExceptionThrown()
        {
            var macro = new MacroDto() { Expression = "$unknownMarkup$" }; //Macro with an unknown markup
            Assert.Throws<InvalidMacroException>(() => this.ComponentUnderTest.Create(macro));
        }

        [Test]
        public void ManageRecord_CreateNewRecord_RecordCreated()
        {
            //Patient with ID 3 exists (Cf. InsertUsers.sql)
            var patient = this.GetPatient(3);

            var cabinet = this.ComponentUnderTest.GetMedicalRecordCabinet(patient);

            var folderCount = cabinet.Folders[0].Records.Count;
            Assert.Greater(cabinet.Folders.Count, 0, "Expecting folders in cabinet");
            Assert.AreEqual(folderCount, 1, "Expecting records in the first folder");

            this.ComponentUnderTest.Create(new MedicalRecordDto() { Tag = cabinet.Folders[0].Records[0].Tag }, patient);

            cabinet = this.ComponentUnderTest.GetMedicalRecordCabinet(patient);
            Assert.Greater(cabinet.Folders.Count, 0, "Expecting folders in cabinet");
            Assert.AreEqual(cabinet.Folders[0].Records.Count, folderCount + 1, "Expecting records in the first folder");
        }

        [Test]
        public void ResolveMacro_WhenMacroIsInvalid_ExceptionThrown()
        {
            var macro = new MacroDto() { Expression = "$unknownMarkup$" }; //Macro with an unknown markup
            var patient = this.HelperComponent.GetAllPatientsLight()[0];

            Assert.Throws<InvalidMacroException>(() => this.ComponentUnderTest.Resolve(macro, patient));
        }

        [Test]
        public void UpdateMacro_WithInvalidExpression_ExceptionThrown()
        {
            var macro = this.ComponentUnderTest.GetAllMacros()[0];
            macro.Expression = "$unknwonMarkup$"; //Macro with an unknown markup
            Assert.Throws<InvalidMacroException>(() => this.ComponentUnderTest.Update(macro));
        }

        [Test]
        public void ValidateMacros_AllMacroAreValid_ValidationSucceeds()
        {
            var macros = this.ComponentUnderTest.GetAllMacros();

            Assert.IsTrue(this.ComponentUnderTest.IsValid(macros));
        }

        [Test]
        public void ValidateMacros_WithAnInvalidMacro_ValidationFails()
        {
            var macros = this.ComponentUnderTest.GetAllMacros().ToList();
            macros.Add(new MacroDto() { Expression = "$unknownExpression$" });

            Assert.IsFalse(this.ComponentUnderTest.IsValid(macros));
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new MedicalRecordComponent(session));
        }

        private LightPatientDto GetPatient(long id)
        {
            return (from p in this.HelperComponent.GetAllPatientsLight()
                    where p.Id == id
                    select p).First();
        }

        #endregion Methods
    }
}