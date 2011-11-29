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

namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private FamilyDto family;
        private IFamilyComponent familyComponent = ObjectFactory.GetInstance<IFamilyComponent>();
        private IMedicalRecordComponent medicalRecordComponent = ObjectFactory.GetInstance<IMedicalRecordComponent>();
        private LightPatientDto selectedChild;
        private LightPatientDto selectedMember;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.MedicalCabinetViewModel = new MedicalCabinetViewModel();
            this.ManageFamilyViewModel = new AddFamilyViewModel();

            this.GetFatherRecordCommand = new RelayCommand(() => this.GetFatherRecord(), () => this.HasFather);
            this.GetMotherRecordCommand = new RelayCommand(() => this.GetMotherRecord(), () => this.HasMother);
        }

        #endregion Constructors

        #region Properties

        public string BtnFather
        {
            get { return Messages.Btn_Father; }
        }

        public string BtnMother
        {
            get { return Messages.Btn_Mother; }
        }

        public bool ChildrenEnabled
        {
            get
            {
                if (this.Family == null) return false;
                else return this.Family.Children.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the family of the selected patient.
        /// </summary>
        /// <value>
        /// The family.
        /// </value>
        public FamilyDto Family
        {
            get { return this.family; }
            set
            {
                this.family = value;
                this.OnPropertyChanged("Family");
            }
        }

        public ICommand GetFatherRecordCommand
        {
            get;
            private set;
        }

        public ICommand GetMotherRecordCommand
        {
            get;
            private set;
        }

        public bool HasFather
        {
            get
            {
                if (this.Family == null) return false;
                else return (this.Family.Father != null);
            }
        }

        public bool HasMother
        {
            get
            {
                if (this.Family == null) return false;
                else return (this.Family.Mother != null);
            }
        }

        public AddFamilyViewModel ManageFamilyViewModel
        {
            get;
            private set;
        }

        public MedicalCabinetViewModel MedicalCabinetViewModel
        {
            get;
            private set;
        }

        public LightPatientDto SelectedChild
        {
            get { return this.selectedChild; }
            set
            {
                if (value != null)
                {
                    this.selectedChild
                        = this.SelectedMember
                        = value;
                    this.GetMedicalRecord();
                    this.OnPropertyChanged("SelectedChild");
                }
            }
        }

        public MedicalRecordCabinetDto SelectedMedicalRecordCabinet
        {
            get { return this.MedicalCabinetViewModel.MedicalRecordCabinet; }
            set
            {
                this.MedicalCabinetViewModel.MedicalRecordCabinet = value;
                this.OnPropertyChanged("SelectedMedicalRecordCabinet");
            }
        }

        public LightPatientDto SelectedMember
        {
            get { return this.selectedMember; }
            set
            {
                this.selectedMember = value;
                this.OnPropertyChanged("SelectedMember");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the data of this instance.
        /// </summary>
        public void Refresh()
        {
            using (this.familyComponent.UnitOfWork)
            {
                this.Family = this.familyComponent.FindFamily(PluginContext.Host.SelectedPatient);
                this.OnPropertyChanged("ChildrenEnabled"); //Notifies children has been updated. Used to (de)activate the combobox
            }
        }

        /// <summary>
        /// Reset the frame to empty. That's set the selected member to null.
        /// </summary>
        public void Reset()
        {
            this.SelectedMember = null;
            this.SelectedMedicalRecordCabinet = null;
            this.SelectedChild = null;
        }

        private void GetFatherRecord()
        {
            this.SelectedMember = this.Family.Father;
            this.GetMedicalRecord();
        }

        private void GetMedicalRecord()
        {
            using (this.medicalRecordComponent.UnitOfWork)
            {
                this.SelectedMedicalRecordCabinet = this.medicalRecordComponent.GetMedicalRecordCabinet(this.SelectedMember);
            }
        }

        private void GetMotherRecord()
        {
            this.SelectedMember = this.Family.Mother;
            this.GetMedicalRecord();
        }

        #endregion Methods
    }
}