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
namespace Probel.NDoctor.Plugins.BmiRecord.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.BmiRecord.Helpers;
    using Probel.NDoctor.Plugins.BmiRecord.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class BmiViewModel : BmiDto
    {
        #region Fields

        private IBmiComponent component;
        private ErrorHandler errorHandler;

        #endregion Fields

        #region Constructors

        public BmiViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
            }
            this.errorHandler = new ErrorHandler(this);
            this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
            this.DeleteCommand = new RelayCommand(() => this.Delete());
        }

        #endregion Constructors

        #region Properties

        public string DateString
        {
            get { return this.Date.ToShortDateString(); }
        }

        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void Delete()
        {
            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskDeleteBmi
                    , Messages.Question
                    , MessageBoxButton.YesNo
                    , MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                using (this.component.UnitOfWork)
                {
                    this.component.RemoveBmiWithDate(PluginContext.Host.SelectedPatient, this.Date);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_BmiDeleted);
                Notifyer.OnItemChanged(this);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_ErrAddBmi);
            }
        }

        #endregion Methods
    }
}