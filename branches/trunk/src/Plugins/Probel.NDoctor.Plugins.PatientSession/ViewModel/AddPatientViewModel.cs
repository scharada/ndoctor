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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddPatientViewModel : BaseViewModel
    {
        #region Fields

        private IPatientSessionComponent component = ComponentFactory.PatientSessionComponent;
        private LightPatientDto patient;

        #endregion Fields

        #region Constructors

        public AddPatientViewModel()
            : base()
        {
            this.Patient = new LightPatientDto();
            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            this.Genders.Add(new Tuple<string, Gender>(Gender.Male.Translate(), Gender.Male));
            this.Genders.Add(new Tuple<string, Gender>(Gender.Female.Translate(), Gender.Female));
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string BtnAdd
        {
            get { return Messages.Btn_Add; }
        }

        public ObservableCollection<Tuple<string, Gender>> Genders
        {
            get;
            private set;
        }

        public LightPatientDto Patient
        {
            get { return this.patient; }
            set
            {
                this.patient = value;
                this.OnPropertyChanged("Patient", "SelectedGender");
            }
        }

        public Tuple<string, Gender> SelectedGender
        {
            get { return new Tuple<string, Gender>(this.Patient.Gender.Translate(), this.Patient.Gender); }
            set
            {
                this.Patient.Gender = value.Item2;
                this.OnPropertyChanged("SelectedGender");
            }
        }

        public string TitleFirstName
        {
            get { return Messages.Title_FirstName; }
        }

        public string TitleGender
        {
            get { return Messages.Title_Gender; }
        }

        public string TitleLastName
        {
            get { return Messages.Title_LastName; }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                using (this.component.UnitOfWork) { this.component.Create(this.Patient); }
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_PatientAdded);
                this.Host.NavigateBack();
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_AddPatient);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Patient.FirstName)
                && !string.IsNullOrWhiteSpace(this.Patient.LastName);
        }

        #endregion Methods
    }
}