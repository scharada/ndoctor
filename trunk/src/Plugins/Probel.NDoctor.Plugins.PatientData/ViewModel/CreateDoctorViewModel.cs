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
namespace Probel.NDoctor.Plugins.PatientData.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Helpers;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class CreateDoctorViewModel : BaseViewModel
    {
        #region Fields

        private IPatientDataComponent component;
        private DoctorDto doctor;
        private Tuple<string, Gender> selectedGender;

        #endregion Fields

        #region Constructors

        public CreateDoctorViewModel()
        {
            this.InitialiseCollections();

            if (!Designer.IsDesignMode) this.component = new ComponentFactory().GetInstance<IPatientDataComponent>();

            this.Doctor = new DoctorDto();
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());

            Notifyer.SpecialisationChanged += (FluentMessageSender, e) => this.Refresh();

            this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public DoctorDto Doctor
        {
            get { return this.doctor; }
            set
            {
                this.doctor = value;
                this.OnPropertyChanged(() => Doctor);
            }
        }

        public ObservableCollection<Tuple<string, Gender>> Genders
        {
            get;
            set;
        }

        public Tuple<string, Gender> SelectedGender
        {
            get { return this.selectedGender; }
            set
            {
                this.Doctor.Gender = value.Item2;
                this.selectedGender = value;
                this.OnPropertyChanged(() => SelectedGender);
            }
        }

        public ObservableCollection<TagDto> Specialisations
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
                    this.component.Create(this.Doctor);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
                this.Doctor = new DoctorDto();
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
            return !string.IsNullOrWhiteSpace(this.Doctor.FirstName)
                && !string.IsNullOrWhiteSpace(this.Doctor.LastName)
                && this.Doctor.Specialisation != null;
        }

        private void InitialiseCollections()
        {
            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            this.Genders.Add(new Tuple<string, Gender>(Gender.Male.Translate(), Gender.Male));
            this.Genders.Add(new Tuple<string, Gender>(Gender.Female.Translate(), Gender.Female));

            this.Specialisations = new ObservableCollection<TagDto>();
        }

        private void Refresh()
        {
            IList<TagDto> result = null;
            using (this.component.UnitOfWork)
            {
                result = this.component.FindTags(TagCategory.Doctor);
            }
            this.Specialisations.Refill(result);
        }

        #endregion Methods
    }
}