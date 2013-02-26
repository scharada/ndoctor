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
namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class AddPathologyViewModel : BaseViewModel
    {
        #region Fields

        private IPathologyComponent component;
        private bool isPopupOpened;
        private bool isTypeEnabled = true;
        private PathologyDto pathology;

        #endregion Fields

        #region Constructors

        public AddPathologyViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
            }
            this.Tags = new ObservableCollection<TagDto>();
            this.Pathology = new PathologyDto();

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
                this.OnPropertyChanged(() => IsPopupOpened);
            }
        }

        public bool IsTypeEnabled
        {
            get { return this.isTypeEnabled; }
            set
            {
                this.isTypeEnabled = value;
                this.OnPropertyChanged(() => IsTypeEnabled);
            }
        }

        public PathologyDto Pathology
        {
            get { return this.pathology; }
            set
            {
                this.pathology = value;
                this.OnPropertyChanged(() => Pathology);
            }
        }

        public ICommand ShowPopupCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        internal void Refresh()
        {
            var result = this.component.GetTags(TagCategory.Pathology);
            this.Tags.Refill(result);
        }

        private void Add()
        {
            try
            {
                this.component.Create(this.Pathology);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Title_OperationDone);
                this.IsPopupOpened = false;
                this.Close();
            }
            catch (ExistingItemException ex)
            {
                this.Handle.Warning(ex, ex.Message);
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_ErrorOccured);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Pathology.Name)
                 && this.pathology.Tag != null;
        }

        #endregion Methods
    }
}