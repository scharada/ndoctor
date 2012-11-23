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
    using System.Threading.Tasks;

    using AutoMapper;

    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private ICalendarComponent component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private DateTime dateToDisplay;
        private bool isBusy;

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

        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(() => IsBusy);
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            this.DateToDisplay = this.DateToDisplay.AddMilliseconds(1);
        }

        private void RefreshCalendar()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var taskContext = new TaskContext() { DateToDisplay = this.DateToDisplay };

            this.IsBusy = true;
            Task.Factory.StartNew<TaskContext>(e => RefreshCalendarAsync(e), taskContext)
            .ContinueWith(e => RefreshCalendarCallback(e), context);
        }

        private TaskContext RefreshCalendarAsync(object e)
        {
            var ctx = e as TaskContext;
            var result = this.component.GetAppointments(ctx.DateToDisplay, ctx.Configuration);
            var mappedResult = Mapper.Map<IList<AppointmentDto>, AppointmentCollection>(result);
            ctx.Result = mappedResult;
            return e as TaskContext;
        }

        private void RefreshCalendarCallback(Task<TaskContext> e)
        {
            this.IsBusy = false;
            base.ExecuteIfTaskIsNotFaulted(e, () =>
            {
                this.DayAppointments.RefillAndSort(e.Result.Result);
            });
        }

        #endregion Methods

        #region Nested Types

        private class TaskContext
        {
            #region Properties

            public GoogleConfiguration Configuration
            {
                get
                {
                    return new PluginSettings().GetGoogleConfiguration();
                }
            }

            public DateTime DateToDisplay
            {
                get;
                set;
            }

            public AppointmentCollection Result
            {
                get;
                set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}