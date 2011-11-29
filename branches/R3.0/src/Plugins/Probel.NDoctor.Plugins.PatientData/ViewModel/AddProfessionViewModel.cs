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
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Helpers;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddProfessionViewModel : BaseViewModel
    {
        #region Fields

        private IPatientDataComponent component;
        private bool isPopupOpened;
        private ProfessionDto profession;

        #endregion Fields

        #region Constructors

        public AddProfessionViewModel()
        {
            this.Profession = new ProfessionDto();

            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.ShowPopupCommand = new RelayCommand(() => this.IsPopupOpened = true);

            if (!Designer.IsDesignMode)
            {
                this.component = ObjectFactory.GetInstance<IPatientDataComponent>();
            }
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public bool IsPopupOpened
        {
            get { return this.isPopupOpened; }
            set
            {
                this.Profession = new ProfessionDto();

                this.isPopupOpened = value;
                this.OnPropertyChanged("IsPopupOpened");
            }
        }

        public ProfessionDto Profession
        {
            get { return this.profession; }
            set
            {
                this.profession = value;
                this.OnPropertyChanged("Profession");
            }
        }

        public ICommand ShowPopupCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.Profession);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Title_OperationDone);
                this.IsPopupOpened = false;
                Notifyer.OnDoctorLinkChanged(this);
            }
            catch (ExistingItemException ex)
            {
                this.HandleWarning(ex, ex.Message);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorOccured);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Profession.Name);
        }

        #endregion Methods
    }
}