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

namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.NDoctor.View.Core.Properties;

    public class LanguageCollection : ObservableCollection<Tuple<string, string>>
    {
        #region Methods

        public static LanguageCollection Build()
        {
            var result = new LanguageCollection();
            result.Add(new Tuple<string, string>(Languages.French, Messages.Language_French));
            result.Add(new Tuple<string, string>(Languages.English, Messages.Language_Englisg));
            return result;
        }

        #endregion Methods
    }

    /// <summary>
    /// Represents all the languages supported in the application
    /// </summary>
    public static class Languages
    {
        #region Fields

        /// <summary>
        /// English language
        /// </summary>
        public static readonly string English = "en";

        /// <summary>
        /// French language
        /// </summary>
        public static readonly string French = "fr";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Determines whether the specified language is supported.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>
        ///   <c>true</c> if the specified language is supported; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSupported(string language)
        {
            return (language == French
                 || language == English);
        }

        #endregion Methods
    }
}