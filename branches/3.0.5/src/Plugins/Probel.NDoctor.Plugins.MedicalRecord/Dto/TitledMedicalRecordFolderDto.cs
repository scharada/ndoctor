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
namespace Probel.NDoctor.Plugins.MedicalRecord.Dto
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class TitledMedicalRecordFolderDto : MedicalRecordFolderDto, ITreeNode
    {
        #region Fields

        private bool isExpanded;
        private bool isSelected;

        #endregion Fields

        #region Constructors

        public TitledMedicalRecordFolderDto()
        {
            this.IsExpanded = true;
        }

        #endregion Constructors

        #region Properties

        public string ContextAddRecord
        {
            get { return Messages.Title_AddRecord; }
        }

        /// <summary>
        /// Gets or sets whether the TreeViewItem
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                this.isExpanded = value;
                this.OnPropertyChanged(() => IsExpanded);
            }
        }

        /// <summary>
        /// Gets/sets whether the TreeViewItem
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged(() => IsSelected);
            }
        }

        public ObservableCollection<TitledMedicalRecordDto> TitledRecords
        {
            get { return TitledMedicalRecordDto.CreateFrom(this.Records).ToObservableCollection(); }
        }

        #endregion Properties

        #region Methods

        public static TitledMedicalRecordFolderDto CreateFrom(MedicalRecordFolderDto folder)
        {
            return Mapper.Map<MedicalRecordFolderDto, TitledMedicalRecordFolderDto>(folder);
        }

        public static IList<TitledMedicalRecordFolderDto> CreateFrom(IList<MedicalRecordFolderDto> folders)
        {
            return Mapper.Map<IList<MedicalRecordFolderDto>, IList<TitledMedicalRecordFolderDto>>(folders);
        }

        #endregion Methods
    }
}