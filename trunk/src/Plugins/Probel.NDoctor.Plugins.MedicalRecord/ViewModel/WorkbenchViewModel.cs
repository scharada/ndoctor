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
namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private ICommand addFolderCommand;
        private ICommand addRecordCommand;
        private TitledMedicalRecordCabinetDto cabinet;
        private IMedicalRecordComponent component = ObjectFactory.GetInstance<IMedicalRecordComponent>();
        private TitledMedicalRecordDto recordToAdd;
        private TitledMedicalRecordDto selectedRecord;
        private IList<TagDto> tags = new List<TagDto>();
        private TagDto tagToAdd;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.RecordToAdd = new TitledMedicalRecordDto() { State = State.Added };
            this.TagToAdd = new TagDto() { Category = TagCategory.MedicalRecord };

            #region Add record
            this.addRecordCommand = new RelayCommand(() =>
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.RecordToAdd, this.Host.SelectedPatient);
                }
                this.Refresh();
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordAdded);
            }, () => this.RecordToAdd.Tag != null);
            #endregion

            #region Add folder
            this.addFolderCommand = new RelayCommand(() =>
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.tagToAdd);
                }
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_TagAdded.StringFormat(this.tagToAdd.Name));
                this.ResetTagToAdd();
                this.Refresh();
            }, () => !string.IsNullOrWhiteSpace(this.tagToAdd.Name));
            #endregion
        }

        #endregion Constructors

        #region Properties

        public ICommand AddFolderCommand
        {
            get { return this.addFolderCommand; }
        }

        public ICommand AddRecordCommand
        {
            get { return this.addRecordCommand; }
        }

        public TitledMedicalRecordCabinetDto Cabinet
        {
            get { return this.cabinet; }
            set
            {
                this.cabinet = value;
                this.OnPropertyChanged("Cabinet");
            }
        }

        public TitledMedicalRecordDto RecordToAdd
        {
            get { return this.recordToAdd; }
            set
            {
                this.recordToAdd = value;
                this.OnPropertyChanged("RecordToAdd");
            }
        }

        public TitledMedicalRecordDto SelectedRecord
        {
            get { return this.selectedRecord; }
            set
            {
                this.selectedRecord = value;
                this.OnPropertyChanged("SelectedRecord");
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagDto> Tags
        {
            get { return this.tags; }
            set
            {
                this.tags = value;
                this.OnPropertyChanged("Tags");
            }
        }

        public TagDto TagToAdd
        {
            get { return this.tagToAdd; }
            set
            {
                this.tagToAdd = value;
                this.OnPropertyChanged("TagToAdd");
            }
        }

        public string TitleAddFolder
        {
            get { return Messages.Title_AddFolder; }
        }

        public string TitleAddRecord
        {
            get { return Messages.Title_AddRecord; }
        }

        public string TitleBtnAdd
        {
            get { return Messages.Title_BtnAdd; }
        }

        public string TitleBtnSearch
        {
            get { return Messages.Btn_SearchFor; }
        }

        public string TitleMedicalRecord
        {
            get { return Messages.Title_MedicalRecord; }
        }

        public string TitleSearchFor
        {
            get { return Messages.Title_SearchFor; }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            Assert.IsNotNull(this.Host);
            Assert.IsNotNull(this.Host.SelectedPatient);

            using (this.component.UnitOfWork)
            {
                var result = this.component.GetMedicalRecordCabinet(this.Host.SelectedPatient);
                this.Cabinet = TitledMedicalRecordCabinetDto.CreateFrom(result);
                this.Tags = this.component.FindTags(TagCategory.MedicalRecord);
            }
        }

        public void Save()
        {
            Assert.IsNotNull(this.Host);
            Assert.IsNotNull(this.Host.SelectedPatient);

            using (this.component.UnitOfWork)
            {
                this.component.UpdateCabinet(this.Host.SelectedPatient, this.Cabinet);
            }
        }

        private void ResetTagToAdd()
        {
            this.tagToAdd.Name = null;
            this.tagToAdd.Notes = null;
            this.tagToAdd.Category = TagCategory.MedicalRecord;
            this.tagToAdd.State = State.Added;
            this.tagToAdd.Id = -1;
        }

        #endregion Methods
    }
}