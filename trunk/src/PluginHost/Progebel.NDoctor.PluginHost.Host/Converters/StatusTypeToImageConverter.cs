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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Progebel.NDoctor.PluginHost.Host.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    using Progebel.Helpers.Assertion;
    using Progebel.NDoctor.PluginHost.Core;

    public class StatusTypeToImageConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Assert.OfType(typeof(StatusType), value);

            var status = (StatusType)value;
            var path = string.Empty;

            switch (status)
            {
                case StatusType.Error:
                    path = "Images/Error.ico";
                    break;
                case StatusType.Info:
                    path = "Images/Info.ico";
                    break;
                case StatusType.Warning:
                    path = "Images/Warning.ico";
                    break;
                default:
                    Assert.FailOnEnumeration(status);
                    break;
            }

            return new BitmapImage(new Uri("/Progebel.NDoctor.PluginHost.Host;component/" + path, UriKind.Relative));
        }

        [Obsolete("Do not use this feature, it is not implemented.")]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This feature is not yet implemented.");
        }

        #endregion Methods
    }
}