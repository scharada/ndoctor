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
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;

    [TestFixture]
    public class PathologyComponentTest : BaseComponentTest<PathologyComponent>
    {
        #region Methods

        /// <summary>
        /// issue 128
        /// </summary>
        [Test]
        public void SearchPathologies_UseJokerSearch_AllPathologiesAreReturned()
        {
            var count1 = this.ComponentUnderTest.GetAllPathologies().Count;
            var count2 = this.ComponentUnderTest.GetPathologiesByName("*").Count;

            Assert.AreEqual(count1, count2);
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new PathologyComponent(session));
        }

        #endregion Methods
    }
}