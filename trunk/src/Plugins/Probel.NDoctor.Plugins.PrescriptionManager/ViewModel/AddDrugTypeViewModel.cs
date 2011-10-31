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

namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddDrugTypeViewModel : BaseViewModel
    {
        #region Fields

        private IPrescriptionComponent component;
        private bool isPopupOpened;
        private TagDto selectedTag;

        #endregion Fields

        #region Constructors

        public AddDrugTypeViewModel()
        {
            if (!Designer.IsDesignMode) this.component = ComponentFactory.PrescriptionComponent;

            this.SelectedTag = new TagDto() { Category = TagCategory.Drug };

            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.ShowPopupCommand = new RelayCommand(() => this.IsPopupOpened = true);
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public bool IsPopupOpened
        {
            get { return this.isPopupOpened; }
            set
            {
                this.isPopupOpened = value;
                this.OnPropertyChanged("IsPopupOpened");
            }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged("SelectedTag");
            }
        }

        public ICommand ShowPopupCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.SelectedTag);
                }
                Notifyer.OnItemChanged(this);
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
                this.SelectedTag = new TagDto() { Category = TagCategory.Drug };
                this.IsPopupOpened = false;
            }
            catch (ExistingItemException ex)
            {
                this.HandleWarning(ex, ex.Message);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorOccured);
            }
        }

        private bool CanAdd()
        {
            return this.SelectedTag != null
                && !string.IsNullOrWhiteSpace(this.SelectedTag.Name)
                && this.SelectedTag.Category == TagCategory.Drug;
        }

        #endregion Methods
    }
}