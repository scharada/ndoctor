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
    using System.Collections.Generic;

    using AutoMapper;

    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private ICalendarComponent component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private DateTime dateToDisplay;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

            this.DateToDisplay = DateTime.Today;
            this.DayAppointments = new AppointmentCollection();

            Notifyer.Refreshed += (sender, e) => this.DateToDisplay = this.DateToDisplay.AddMilliseconds(1);
            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "DateToDisplay")
                    this.RefreshCalendar();
            };
        }

        #endregion Constructors

        #region Properties

        public DateTime DateToDisplay
        {
            get { return this.dateToDisplay; }
            set
            {
                this.dateToDisplay = value;
                this.OnPropertyChanged(() => DateToDisplay);
            }
        }

        public AppointmentCollection DayAppointments
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void RefreshCalendar()
        {
            try
            {
                var result = this.component.FindAppointments(this.DateToDisplay);
                var mappedResult = Mapper.Map<IList<AppointmentDto>, AppointmentCollection>(result);
                this.DayAppointments.Refill(mappedResult);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_RefreshingCalendar);
            }
        }

        #endregion Methods
    }
}