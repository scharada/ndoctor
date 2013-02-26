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

namespace Probel.NDoctor.Plugins.PatientSession.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Plugins.PatientSession.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;

    internal class SettingsViewModel : PluginSettingsViewModel
    {
        #region Fields

        private readonly PluginSettings Settings = new PluginSettings();

        #endregion Fields

        #region Properties

        public bool ShowBirthdate
        {
            get { return this.Settings.ShowBirthdate; }
            set
            {
                this.Settings.ShowBirthdate = value;
                this.OnPropertyChanged(() => ShowBirthdate);
            }
        }

        public bool ShowCity
        {
            get { return this.Settings.ShowCity; }
            set
            {
                this.Settings.ShowCity = value;
                this.OnPropertyChanged(() => ShowCity);
            }
        }

        public bool ShowInscriptionDate
        {
            get { return this.Settings.ShowInscription; }
            set
            {
                this.Settings.ShowInscription = value;
                this.OnPropertyChanged(() => ShowInscriptionDate);
            }
        }

        public bool ShowLastUpdate
        {
            get { return this.Settings.ShowLastUpdate; }
            set
            {
                this.Settings.ShowLastUpdate = value;
                this.OnPropertyChanged(() => ShowLastUpdate);
            }
        }

        public bool ShowProfession
        {
            get { return this.Settings.ShowProfession; }
            set
            {
                this.Settings.ShowProfession = value;
                this.OnPropertyChanged(() => ShowProfession);
            }
        }

        public bool ShowReason
        {
            get { return this.Settings.SHowReason; }
            set
            {
                this.Settings.SHowReason = value;
                this.OnPropertyChanged(() => ShowReason);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether this instance can save.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can save; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanSave()
        {
            return true;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        protected override void Save()
        {
            this.Settings.Save();
        }

        #endregion Methods
    }
}