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

namespace Probel.NDoctor.Plugins.PatientOverview.Translations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Plugins.PatientOverview.Properties;

    internal static class TagText
    {
        #region Properties

        public static string AddTagTitle
        {
            get { return Messages.Title_AddTag; }
        }

        public static string BindTagTitle
        {
            get { return Messages.Title_BindTag; }
        }

        public static string Explanations
        {
            get { return Messages.Lbl_Explanations; }
        }

        public static string Name
        {
            get { return Messages.Lbl_Name; }
        }

        public static string RemoveTagTitle
        {
            get { return Messages.Title_RemoveTag; }
        }

        #endregion Properties
    }
}