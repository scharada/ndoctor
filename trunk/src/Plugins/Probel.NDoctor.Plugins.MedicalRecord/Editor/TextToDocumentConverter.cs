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

namespace Probel.NDoctor.Plugins.MedicalRecord.Editor
{
    using System;
    using System.Windows.Data;

    using ICSharpCode.AvalonEdit.Document;

    using Probel.NDoctor.Domain.DTO.Objects;

    [ValueConversion(typeof(string), typeof(TextDocument))]
    public class TextToDocumentConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                var str = value as string;
                var result = new TextDocument();
                result.Text = str;
                return result;
            }
            else { return new TextDocument(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TextDocument)
            {
                var document = value as TextDocument;
                return document.Text;
            }
            else { return string.Empty; }
        }

        #endregion Methods
    }
}