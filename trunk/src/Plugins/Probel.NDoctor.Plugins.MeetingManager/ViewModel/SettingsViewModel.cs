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

namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;

    internal class SettingsViewModel : PluginSettingsViewModel
    {
        #region Fields

        private readonly PluginSettings Settings = new PluginSettings();

        #endregion Fields

        #region Properties

        public string BindedCalendar
        {
            get { return this.Settings.BindedCalendar; }
            set
            {
                this.Settings.BindedCalendar = value;
                this.OnPropertyChanged(() => BindedCalendar);
            }
        }

        public bool IsGoogleCalendarActivated
        {
            get { return this.Settings.IsGoogleCalendarActivated; }
            set
            {
                this.Settings.IsGoogleCalendarActivated = value;
                this.OnPropertyChanged(() => IsGoogleCalendarActivated);
            }
        }

        public string Password
        {
            get { return this.Settings.Password; }
            set
            {
                this.Settings.Password = value;
                this.OnPropertyChanged(() => Password);
            }
        }

        public string UserName
        {
            get { return this.Settings.UserName; }
            set
            {
                this.Settings.UserName = value;
                this.OnPropertyChanged(() => UserName);
            }
        }

        #endregion Properties

        #region Methods

        protected override bool CanSave()
        {
            return true;
        }

        protected override void Save()
        {
            this.Settings.Save();
        }

        #endregion Methods
    }
}