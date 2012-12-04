namespace Probel.NDoctor.Domain.Test.Components
{
    using System.Drawing;

    using NUnit.Framework;

    using Probel.Helpers.Conversions;
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
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class PictureComponentTest : BaseComponentTest<PictureComponent>
    {
        #region Properties

        private PictureDto EmptyPicture
        {
            get
            {
                return new PictureDto()
                {
                    Bitmap = Converter.ImageToByteArray(new Bitmap(10, 10)),
                    Tag = this.ComponentUnderTest.GetTags(TagCategory.Picture)[0],
                };
            }
        }

        #endregion Properties

        #region Methods

        [Test]
        public void UpdatePicture_ChangeTag_TheChangedTagIsNotAdded()
        {
            var pic = this.EmptyPicture;
            var patient = this.HelperComponent.GetAllPatientsLight()[0];
            var tagCount = this.ComponentUnderTest.GetTags(TagCategory.Picture).Count;

            this.WrapInTransaction(() => this.ComponentUnderTest.Create(pic, patient));
            this.WrapInTransaction(() => this.ComponentUnderTest.Update(GetFirstPicture(patient)));

            Assert.AreEqual(tagCount, this.ComponentUnderTest.GetTags(TagCategory.Picture).Count);
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new PictureComponent(session));
        }

        private PictureDto GetFirstPicture(LightPatientDto patient)
        {
            return this.ComponentUnderTest.GetPictures(patient)[0];
        }

        #endregion Methods
    }
}