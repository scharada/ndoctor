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

namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddPathologyViewModel : BaseBoxViewModel<PathologyDto>
    {
        #region Fields

        private bool isTypeEnabled;

        #endregion Fields

        #region Constructors

        public AddPathologyViewModel()
        {
            this.Tags = new ObservableCollection<TagDto>();
            this.BoxItem = new PathologyDto();
            this.IsTypeEnabled = true;
        }

        #endregion Constructors

        #region Properties

        public bool IsTypeEnabled
        {
            get { return this.isTypeEnabled; }
            set
            {
                this.isTypeEnabled = value;
                this.OnPropertyChanged(() => IsTypeEnabled);
            }
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                var tags = this.Component.GetTags(TagCategory.Pathology);
                this.Tags.Refill(tags);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        protected override void AddItem()
        {
            this.Component.Create(this.BoxItem);
        }

        #endregion Methods
    }
}