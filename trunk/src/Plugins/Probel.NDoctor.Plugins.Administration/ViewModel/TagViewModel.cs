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
namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Collections;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Helpers;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.Domain.Components;

    public class TagViewModel : TagDto
    {
        #region Fields

        private IAdministrationComponent component;
        private ErrorHandler errorHandler;

        #endregion Fields

        #region Constructors

        public TagViewModel()
            : base(TagCategory.Appointment)
        {
            if (!Designer.IsDesignMode) this.component = new ComponentFactory().GetInstance<IAdministrationComponent>();
            this.errorHandler = new ErrorHandler(this);
            this.Categories = TagCategoryCollection.Build();

            this.UpdateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
        }

        #endregion Constructors

        #region Properties

        public TagCategoryCollection Categories
        {
            get;
            private set;
        }

        public IAdministrationComponent ObjectFatory
        {
            get;
            set;
        }

        public Tuple<string, TagCategory> SelectedTag
        {
            get { return new Tuple<string, TagCategory>(this.Category.Translate(), this.Category); }
            set
            {
                this.Category = value.Item2;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanUpdate()
        {
            return !string.IsNullOrWhiteSpace(this.Name);
        }

        private void Update()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Update(this);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DataSaved);
                Notifyer.OnTagsChanged(this);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_ErrorOccured);
            }
        }

        #endregion Methods
    }
}