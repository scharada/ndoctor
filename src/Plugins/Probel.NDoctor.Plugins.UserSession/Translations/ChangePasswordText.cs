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
namespace Probel.NDoctor.Plugins.UserSession.Translations
{
    using Probel.NDoctor.Plugins.UserSession.Properties;

    public static class ChangePasswordText
    {
        #region Properties

        public static string Title
        {
            get { return Messages.Title_ChangePwd; }
        }

        public static string TitleAgainPwd
        {
            get { return Messages.Title_RepeatPasword; }
        }

        public static string TitleChangePwd
        {
            get { return Messages.Title_ChangePwd; }
        }

        public static string TitleOldPassword
        {
            get
            {
                return Messages.Title_OldPwd;
            }
        }

        public static string TitlePassword
        {
            get
            {
                return Messages.Title_Password;
            }
        }

        public static string TitleValidatNewPwd
        {
            get { return Messages.Title_ValidatePwdChange; }
        }

        #endregion Properties
    }
}