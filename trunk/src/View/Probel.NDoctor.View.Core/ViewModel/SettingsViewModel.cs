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

namespace Probel.NDoctor.View.Core.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class SettingsViewModel : BaseViewModel
    {
        #region Fields

        private SettingUi selectedControl;

        #endregion Fields

        #region Constructors

        public SettingsViewModel()
        {
            this.SettingCollection = new ObservableCollection<SettingUi>(new SettingsConfigurator().Controls);
            this.SaveSettingsCommand = new RelayCommand(() => this.SaveSettings(), () => this.CanSaveSettings());
        }

        #endregion Constructors

        #region Properties

        public ICommand SaveSettingsCommand
        {
            get;
            private set;
        }

        public SettingUi SelectedControl
        {
            get { return this.selectedControl; }
            set
            {
                this.selectedControl = value;
                this.OnPropertyChanged(() => SelectedControl);
            }
        }

        public ObservableCollection<SettingUi> SettingCollection
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanSaveSettings()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void SaveSettings()
        {
            var hasError = false;
            foreach (var ui in this.SettingCollection)
            {
                if (ui.Control.DataContext is PluginSettingsViewModel)
                {
                    var vm = ui.Control.DataContext as PluginSettingsViewModel;
                    if (!vm.SaveCommand.CanExecute(null)) { hasError = true; }

                    vm.SaveCommand.TryExecute();
                }
            }
            if (!hasError) { this.Close(); }
        }

        #endregion Methods
    }
}