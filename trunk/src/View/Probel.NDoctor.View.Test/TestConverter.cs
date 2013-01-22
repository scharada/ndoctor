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

namespace Probel.NDoctor.View.Test
{
    using System;
    using System.Threading;
    using System.Windows;

    using NSubstitute;

    using NUnit.Framework;

    using Probel.NDoctor.View.Toolbox.Converters;

    [TestFixture]
    public class TestConverter
    {
        #region Methods

        [Test]
        public void ConvertStringToVisibility_WithEmptyString_Collapsed()
        {
            Assert.AreEqual(Visibility.Collapsed, this.Convert(string.Empty));
        }

        [Test]
        public void ConvertStringToVisibility_WithNonEmptyString_Visible()
        {
            Assert.AreEqual(Visibility.Visible, this.Convert("hello world"));
        }

        [Test]
        public void ConvertStringToVisibility_WithNullString_Collapsed()
        {
            Assert.AreEqual(Visibility.Collapsed, this.Convert(null));
        }

        [Test]
        public void ConvertStringToVisibility_WithOnlyWhiteSpace_Collapsed()
        {
            Assert.AreEqual(Visibility.Collapsed, this.Convert(" "));
        }

        private Visibility Convert(string toConvert)
        {
            var converter = new StringToVisibilityConverter();
            return (Visibility)converter.Convert(toConvert, Substitute.For<Type>(), Substitute.For<object>(), Thread.CurrentThread.CurrentCulture);
        }

        #endregion Methods
    }
}