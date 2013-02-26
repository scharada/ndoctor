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
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class AddRecordViewModel : BaseViewModel
    {
        #region Fields

        private ICommand addRecordCommand;
        private IMedicalRecordComponent component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private MedicalRecordDto recordToAdd;
        private TagDto selectedTag;
        private ObservableCollection<TagDto> tags = new ObservableCollection<TagDto>();

        #endregion Fields

        #region Constructors

        public AddRecordViewModel()
        {
            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
            this.recordToAdd = new MedicalRecordDto();
            this.addRecordCommand = new RelayCommand(() => this.AddRecord(), () => this.CanAddRecord());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddRecordCommand
        {
            get { return this.addRecordCommand; }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ObservableCollection<TagDto> Tags
        {
            get { return this.tags; }
            set
            {
                this.tags = value;
                this.OnPropertyChanged(() => this.Tags);
            }
        }

        public string Title
        {
            get { return this.recordToAdd.Name; }
            set
            {
                this.recordToAdd.Name = value;
                this.OnPropertyChanged(() => this.Title);
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                this.Tags.Refill(this.component.GetTags(TagCategory.MedicalRecord));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void AddRecord()
        {
            try
            {
                this.recordToAdd.Tag = this.SelectedTag;
                this.recordToAdd.Rtf = string.Empty;
                this.component.Create(this.recordToAdd, PluginContext.Host.SelectedPatient);

                this.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordAdded);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
            finally { this.Close(); }
        }

        private bool CanAddRecord()
        {
            return this.selectedTag != null;
        }

        #endregion Methods
    }
}