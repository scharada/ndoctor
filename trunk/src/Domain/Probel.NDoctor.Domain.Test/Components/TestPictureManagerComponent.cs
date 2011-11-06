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
namespace Probel.NDoctor.Domain.Test.Component
{
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestPictureManagerComponent : TestBase<PictureComponent>
    {
        #region Methods

        [Test]
        public void CanAddNewPicture()
        {
            var patients = this.Component.GetAllPatientsLight();

            var pic = new PictureDto()
            {
                Bitmap = Build.Picture(1),
                Notes = "Some notes",
                Tag = new TagDto() { Name = "SomeTag" },
            };

            this.Component.Create(pic, patients[0]);

            var pics = this.Component.FindPictures(patients[0]);
            Assert.Greater(pics.Count, 0);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override PictureComponent GetComponentInstance()
        {
            return new PictureComponent(Database.Scope.OpenSession());
        }

        #endregion Methods
    }
}