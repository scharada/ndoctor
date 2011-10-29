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

    using AutoMapper;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class TitledMedicalRecordDto : MedicalRecordDto, ITreeNode
    {
        #region Fields

        private bool isExpanded;
        private bool isSelected;

        #endregion Fields

        #region Constructors

        public TitledMedicalRecordDto()
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
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        #endregion Properties

        #region Methods

        public static TitledMedicalRecordDto CreateFrom(MedicalRecordDto record)
        {
            return Mapper.Map<MedicalRecordDto, TitledMedicalRecordDto>(record);
        }

        public static IList<TitledMedicalRecordDto> CreateFrom(IList<MedicalRecordDto> records)
        {
            return Mapper.Map<IList<MedicalRecordDto>, IList<TitledMedicalRecordDto>>(records);
        }

        #endregion Methods
    }
}