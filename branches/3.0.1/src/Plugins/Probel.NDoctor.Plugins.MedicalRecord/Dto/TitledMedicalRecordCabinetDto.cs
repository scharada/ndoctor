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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class TitledMedicalRecordCabinetDto : MedicalRecordCabinetDto, ITreeNode
    {
        #region Fields

        private bool isExpanded;

        #endregion Fields

        #region Constructors

        public TitledMedicalRecordCabinetDto()
        {
            this.IsExpanded = true;
        }

        #endregion Constructors

        #region Properties

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
                this.OnPropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// Gets/sets whether the TreeViewItem
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return Messages.Title_SearchFor;
            }
        }

        public ObservableCollection<TitledMedicalRecordFolderDto> TitledFolders
        {
            get { return TitledMedicalRecordFolderDto.CreateFrom(this.Folders).ToObservableCollection(); }
        }

        #endregion Properties

        #region Methods

        public static TitledMedicalRecordCabinetDto CreateFrom(MedicalRecordCabinetDto cabinet)
        {
            return Mapper.Map<MedicalRecordCabinetDto, TitledMedicalRecordCabinetDto>(cabinet);
        }

        public static IList<TitledMedicalRecordCabinetDto> CreateFrom(IList<MedicalRecordDto> cabinets)
        {
            return Mapper.Map<IList<MedicalRecordDto>, IList<TitledMedicalRecordCabinetDto>>(cabinets);
        }

        /// <summary>
        /// Will loop on all the records and apply the specified action if the specified selector returns <c>True</c>.
        /// </summary>
        /// <param name="action">The action to execute on the records that fulfill the selection action.</param>
        /// <param name="selector">The selector returns <c>True</c> if the current records should be selected. Otherwise returns <c>False</c></param>
        public void ForEachRecord(Action<MedicalRecordDto> action, Func<MedicalRecordDto, bool> selector)
        {
            foreach (var folder in this.Folders)
                foreach (var record in folder.Records)
                {
                    if (selector(record)) action(record);
                }
        }

        #endregion Methods
    }
}