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

namespace Probel.NDoctor.Plugins.PictureManager.Translations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Plugins.PictureManager.Properties;

    internal static class AddPictureText
    {
        #region Properties

        public static string BtnSelect
        {
            get { return Messages.Btn_ChoosePicture; }
        }

        public static string GroupBox
        {
            get { return Messages.Gb_Picture; }
        }

        public static string LblCategory
        {
            get { return Messages.Lbl_Category; }
        }

        public static string LblNotes
        {
            get { return Messages.Lbl_Notes; }
        }

        public static string Title
        {
            get { return Messages.Title_BtnAddPic; }
        }

        public static string TitleCategory
        {
            get { return Messages.Title_AddPicType; }
        }

        #endregion Properties
    }
}