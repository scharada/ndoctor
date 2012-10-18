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
namespace Probel.NDoctor.View.Toolbox.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Probel.Helpers.Assertion;

    /// <summary>
    /// Invert the specified visibility. That's convert 
    ///  * a Visibility.Visible into Visibility.Collapsed
    ///  * a Visibility.Collapsed or Visibility.Hidden into Visibility.Visible
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    public class InvertVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Assert.OfType<Visibility>(value);
            var visibility = (Visibility)value;

            var result = Visibility.Visible;
            switch (visibility)
            {
                case Visibility.Collapsed:
                case Visibility.Hidden:
                    result = Visibility.Visible;
                    break;
                case Visibility.Visible:
                    result = Visibility.Collapsed;
                    break;
                default:
                    Assert.FailOnEnumeration(visibility);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        [Obsolete("Not used")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Not used");
        }

        #endregion Methods
    }
}