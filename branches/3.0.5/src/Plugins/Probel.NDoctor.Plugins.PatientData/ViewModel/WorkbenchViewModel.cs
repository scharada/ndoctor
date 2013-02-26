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
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.Mvvm.Gui.FileServices;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IPatientDataComponent component;
        private PatientDto patient;
        private LightDoctorDto selectedDoctor;
        private byte[] thumbnail;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();

            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();

            this.InitialiseCollections();

            this.ChangeImageCommand = new RelayCommand(() => this.ChangeImage(), () => this.CanChangePicture());
            this.BindDoctorCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<BindDoctorViewModel>(), () => this.Patient != null);
            this.RemoveLinkCommand = new RelayCommand(() => this.RemoveLink());

            this.SaveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
        }

        #endregion Constructors

        #region Properties

        public ICommand BindDoctorCommand
        {
            get;
            private set;
        }

        public ICommand ChangeImageCommand
        {
            get;
            private set;
        }

        public ObservableCollection<LightDoctorDto> Doctors
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
                this.OnPropertyChanged(() => this.Patient);
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

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public LightDoctorDto SelectedDoctor
        {
            get { return this.selectedDoctor; }
            set
            {
                this.selectedDoctor = value;
                this.OnPropertyChanged(() => SelectedDoctor);
            }
        }

        public byte[] Thumbnail
        {
            get { return this.thumbnail; }
            set
            {
                this.thumbnail = value;
                this.OnPropertyChanged(() => Thumbnail);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the data of this instance.
        /// </summary>        
        public void Refresh()
        {
            try
            {
                if (PluginContext.Host.SelectedPatient == null) { return; }

                PluginContext.Host.SelectedPatient = this.component.GetLightPatientById(PluginContext.Host.SelectedPatient.Id);
                var result = this.component.GetDoctorOf(PluginContext.Host.SelectedPatient);
                this.Doctors.Refill(result);

                //Refill the collections BEFORE refreshing the patient.
                this.Insurances.Refill(this.component.GetAllInsurancesLight());
                this.Practices.Refill(this.component.GetAllPracticesLight());
                this.Reputations.Refill(this.component.GetAllReputations());
                this.Professions.Refill(this.component.GetAllProfessions());

                //Refresh the patient with the refreshed collection binding
                this.Patient = this.component.GetPatient(PluginContext.Host.SelectedPatient);
                this.RefreshPatientData();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanChangePicture()
        {
            return this.Patient != null;
        }

        private bool CanSave()
        {
            return this.Patient != null
                && this.Patient.IsValid()
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void ChangeImage()
        {
            var file = string.Empty;
            var option = new Options()
            {
                Multiselect = false,
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF"
            };
            var dr = FileGuiFactory.Win32.SelectFile(e => file = e, option);

            if (dr == true && File.Exists(file))
            {
                var img = Image.FromFile(file);
                Task.Factory.StartNew(e => this.UpdateThumbnail(e), new { Patient = this.Patient, Thumbnail = img.GetThumbnail() });
            }
        }

        private void InitialiseCollections()
        {
            this.Insurances = new ObservableCollection<LightInsuranceDto>();
            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Practices = new ObservableCollection<LightPracticeDto>();
            this.Doctors = new ObservableCollection<LightDoctorDto>();

            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            this.Genders.Add(new Tuple<string, Gender>(Gender.Male.Translate(), Gender.Male));
            this.Genders.Add(new Tuple<string, Gender>(Gender.Female.Translate(), Gender.Female));
        }

        private void RefreshPatientData()
        {
            if (this.Patient.Insurance != null)
            {
                this.Patient.Insurance = (from i in this.Insurances
                                          where i.Id == this.Patient.Insurance.Id
                                          select i).FirstOrDefault();
            }

            if (this.Patient.Practice != null)
            {

                this.Patient.Practice = (from p in this.Practices
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

            try
            {
                this.Thumbnail = this.component.GetThumbnail(this.Patient);
            }
            catch (Exception ex) { this.Handle.Warning(ex, Messages.Warn_ImpossibleToDisplayThumb); }
        }

        private void RemoveLink()
        {
            try
            {
                var dr = ViewService.MessageBox.Question(Messages.Msg_AskRemoveLink);
                if (!dr) return;

                this.component.RemoveDoctorFor(PluginContext.Host.SelectedPatient, this.SelectedDoctor);

                this.Refresh();

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoctorRemoved);
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_ErrorRemovingDoctor);
            }
        }

        private void Save()
        {
            try
            {
                this.component.Update(this.Patient);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void UpdateThumbnail(dynamic context)
        {
            this.component.UpdateThumbnail(context.Patient, context.Thumbnail);
        }

        #endregion Methods
    }
}