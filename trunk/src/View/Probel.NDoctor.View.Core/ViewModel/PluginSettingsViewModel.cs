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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public abstract class PluginSettingsViewModel : BaseViewModel
    {
        #region Constructors

        public PluginSettingsViewModel()
        {
            this.SaveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            Notifyer.SavingSettings += (sender, e) => this.SaveCommand.TryExecute();
        }

        #endregion Constructors

        #region Properties

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected abstract bool CanSave();

        protected abstract void Save();

        #endregion Methods
    }
}