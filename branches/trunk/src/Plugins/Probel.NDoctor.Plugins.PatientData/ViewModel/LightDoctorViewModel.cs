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

namespace Probel.NDoctor.Plugins.PatientData.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Helpers;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class LightDoctorViewModel : LightDoctorDto
    {
        #region Fields

        private IPatientDataComponent component;
        private ErrorHandler errorHandler;
        private bool isSelected;

        #endregion Fields

        #region Constructors

        public LightDoctorViewModel()
        {
            this.component = ComponentFactory.PatientDataComponent;
            this.errorHandler = new ErrorHandler(this);
            this.AddDoctorCommand = new RelayCommand(() => this.AddDoctor());
            this.RemoveLinkCommand = new RelayCommand(() => this.RemoveLink());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddDoctorCommand
        {
            get;
            private set;
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public ICommand RemoveLinkCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void AddDoctor()
        {
            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskAddDoctor
                     , Messages.Question
                     , MessageBoxButton.YesNo
                     , MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                using (this.component.UnitOfWork)
                {
                    this.component.AddLink(PluginContext.Host.SelectedPatient, (LightDoctorDto)this);
                }
                Notifyer.OnDoctorLinkChanged(this);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoctorAded);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_ErrorAddDoctor);
            }
        }

        private void RemoveLink()
        {
            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskRemoveLink
                    , Messages.Question
                    , MessageBoxButton.YesNo
                    , MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                using (this.component.UnitOfWork)
                {
                    this.component.RemoveLink(PluginContext.Host.SelectedPatient, (LightDoctorDto)this);
                }
                Notifyer.OnDoctorLinkChanged(this);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoctorRemoved);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_ErrorRemovingDoctor);
            }
        }

        #endregion Methods
    }
}