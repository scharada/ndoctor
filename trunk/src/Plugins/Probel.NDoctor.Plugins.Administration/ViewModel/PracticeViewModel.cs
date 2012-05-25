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
namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class PracticeViewModel : PracticeDto
    {
        #region Fields

        private IAdministrationComponent component;
        private ErrorHandler errorHandler;

        #endregion Fields

        #region Constructors

        public PracticeViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            }
            this.errorHandler = new ErrorHandler(this);
            this.UpdateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
        }

        #endregion Constructors

        #region Properties

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanUpdate()
        {
            return !string.IsNullOrWhiteSpace(this.Name) && PluginContext.DoorKeeper.Grants(To.Write);
        }

        private void Update()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Update(this);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_ErrorOccured);
            }
        }

        #endregion Methods
    }
}