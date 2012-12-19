namespace Probel.NDoctor.Domain.Test.Components
{
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
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class PrescriptionComponentTest : BaseComponentTest<PrescriptionComponent>
    {
        #region Methods

        /// <summary>
        /// issue 129
        /// </summary>
        [Test]
        public void CreatePrescription_WithTwiceSameDrug_NoError()
        {
            var user = new PatientSessionComponent(this.Session).GetPatientsByNameLight("*", SearchOn.FirstAndLastName)[0];
            var drugs = this.ComponentUnderTest.GetDrugsByName("*");

            var document = new PrescriptionDocumentDto();
            var prescription = new PrescriptionDto();
            document.Prescriptions.Add(new PrescriptionDto() { Drug = drugs[0] });
            document.Prescriptions.Add(new PrescriptionDto() { Drug = drugs[0] });

            this.WrapInTransaction(() => this.ComponentUnderTest.Create(document, user));
        }

        [Test]
        public void FindDrugs_UsingJokerSearch_FindAllDrugs()
        {
            var drugs = this.ComponentUnderTest.GetDrugsByName("*");

            Assert.Greater(drugs.Count, 0);
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new PrescriptionComponent(session));
        }

        private DrugDto Clone(DrugDto drug)
        {
            return new DrugDto()
            {
                Id = drug.Id,
                IsImported = drug.IsImported,
                Name = drug.Name,
                Notes = drug.Notes,
                Segretator = drug.Segretator,
                Tag = new TagDto(drug.Tag.Category)
                {
                    Id = drug.Tag.Id,
                    IsImported = drug.Tag.IsImported,
                    Name = drug.Tag.Name,
                    Notes = drug.Tag.Notes,
                    Segretator = drug.Tag.Segretator,
                }
            };
        }

        #endregion Methods
    }
}