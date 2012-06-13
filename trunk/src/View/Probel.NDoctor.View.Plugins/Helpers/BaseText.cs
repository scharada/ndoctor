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

namespace Probel.NDoctor.View.Plugins.Helpers
{
    using Probel.NDoctor.View.Plugins.Properties;

    public static class BaseText
    {
        #region Properties

        public static string Add
        {
            get { return Messages.Btn_Add; }
        }

        public static string Cancel
        {
            get { return Messages.Btn_Cancel; }
        }

        public static string Error
        {
            get { return Messages.Title_Error; }
        }

        public static string Information
        {
            get { return Messages.Title_Information; }
        }

        public static string OK
        {
            get { return Messages.Btn_OK; }
        }

        public static string Question
        {
            get { return Messages.Question; }
        }

        public static string Question_Delete
        {
            get { return Messages.Question_Delete; }
        }

        public static string Warning
        {
            get { return Messages.Title_Warning; }
        }

        #endregion Properties


        public static string Edit
        {
            get { return Messages.Title_Edit; }
        }
    }
}