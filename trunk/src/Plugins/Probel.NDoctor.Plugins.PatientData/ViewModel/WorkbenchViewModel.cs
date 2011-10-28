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
    using System.Linq;
    using System.Windows.Input;

    using AutoMapper;

    using Microsoft.Win32;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Helpers;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IPatientDataComponent component;
        private PatientDto patient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.component = ObjectFactory.GetInstance<IPatientDataComponent>();
            Notifyer.DoctorLinkChanged += (sender, e) => this.Refresh();

            this.InitialiseCollections();

            this.ChangeImageCommand = new RelayCommand(() => this.ChangeImage(), () => this.CanChangePicture());
        }

        #endregion Constructors

        #region Properties

        public ICommand ChangeImageCommand
        {
            get;
            private set;
        }

        public ObservableCollection<LightDoctorViewModel> Doctors
        {
            get;
            private set;
        }

        public ObservableCollection<Tuple<string, Gender>> Genders
        {
            get;
            set;
        }

        public ObservableCollection<LightInsuranceDto> Insurances
        {
            get;
            set;
        }

        public PatientDto Patient
        {
            get { return this.patient; }
            set
            {
                this.patient = value;
                this.OnPropertyChanged("Patient", "SelectedGender");
            }
        }

        public ObservableCollection<LightPracticeDto> Practices
        {
            get;
            set;
        }

        public ObservableCollection<ProfessionDto> Professions
        {
            get;
            set;
        }

        public ICommand RemoveLinkCommand
        {
            get;
            private set;
        }

        public ObservableCollection<ReputationDto> Reputations
        {
            get;
            set;
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the data of this instance.
        /// </summary>
        public void Refresh()
        {
            if (this.Host.SelectedPatient == null) return;

            using (this.component.UnitOfWork)
            {
                var result = this.component.GetDoctorOf(this.Host.SelectedPatient);
                var mapped = Mapper.Map<IList<LightDoctorDto>, IList<LightDoctorViewModel>>(result);
                this.Doctors.Refill(mapped);

                this.Insurances.Refill(this.component.GetAllInsurancesLight());
                this.Practices.Refill(this.component.GetAllPracticesLight());
                this.Reputations.Refill(this.component.GetAllReputations());
                this.Professions.Refill(this.component.GetAllProfessions());

                //Refill the collections BEFORE refreshing the patient.
                this.Patient = this.component.GetPatient(this.Host.SelectedPatient);
                if (this.Patient.Insurance != null)
                {
                    this.Patient.Insurance = (from i in this.Insurances
                                              where i.Id == this.Patient.Insurance.Id
                                              select i).FirstOrDefault();
                }
                if (this.Patient.Practice != null)
                {

                    this.patient.Practice = (from p in this.Practices
                                             where p.Id == this.Patient.Practice.Id
                                             select p).FirstOrDefault();
                }

                if (this.Patient.Reputation != null)
                {
                    this.Patient.Reputation = (from r in this.Reputations
                                               where r.Id == this.Patient.Reputation.Id
                                               select r).FirstOrDefault();
                }

                if (this.Patient.Profession != null)
                {
                    this.Patient.Profession = (from p in this.Professions
                                               where p.Id == this.Patient.Profession.Id
                                               select p).FirstOrDefault();
                }
            }
        }

        public void Save()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Update(this.Patient);
                }
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorSave, ex.Message);
            }
        }

        private bool CanChangePicture()
        {
            return this.Patient != null;
        }

        private void ChangeImage()
        {
            var dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF",
            };
            var clickedOK = dialog.ShowDialog();
            if (clickedOK.HasValue && clickedOK.Value)
            {
                var bytes = Converter.FileToByteArray(dialog.FileName);
                this.Patient.Thumbnail = bytes;
            }
            else return;
        }

        private void InitialiseCollections()
        {
            this.Insurances = new ObservableCollection<LightInsuranceDto>();
            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Practices = new ObservableCollection<LightPracticeDto>();
            this.Doctors = new ObservableCollection<LightDoctorViewModel>();

            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            this.Genders.Add(new Tuple<string, Gender>(Gender.Male.Translate(), Gender.Male));
            this.Genders.Add(new Tuple<string, Gender>(Gender.Female.Translate(), Gender.Female));
        }

        #endregion Methods
    }
}