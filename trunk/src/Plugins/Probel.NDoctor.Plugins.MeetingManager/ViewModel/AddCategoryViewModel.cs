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
namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddCategoryViewModel : BaseViewModel
    {
        #region Fields

        private static readonly ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

        private string value;

        #endregion Fields

        #region Constructors

        public AddCategoryViewModel()
        {
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.OnPropertyChanged(() => Value);
            }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                var tag = new TagDto(TagCategory.Appointment) { Name = this.Value, };

                Component.Create(tag);

                InnerWindow.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_CategoryAdded);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Value)
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        #endregion Methods
    }
}