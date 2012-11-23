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

namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Windows.Media;

    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;

    internal class SettingsViewModel : PluginSettingsViewModel
    {
        #region Fields

        public readonly PluginSettings Configuration = new PluginSettings();

        #endregion Fields

        #region Properties

        public FontFamily FontFamily
        {
            get { return this.Configuration.FontFamily; }
            set
            {
                this.Configuration.FontFamily = value;
                this.OnPropertyChanged(() => FontFamily);
            }
        }

        public int FontSize
        {
            get
            {
                return this.Configuration.FontSize;
            }
            set
            {
                this.Configuration.FontSize = value;
                this.OnPropertyChanged(() => FontSize);
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
            this.Configuration.Save();
        }

        #endregion Methods
    }
}