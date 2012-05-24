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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using log4net;

    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class DateRangeViewModel : DateRange, INotifyPropertyChanged, IErrorHandler
    {
        #region Fields

        private static IList<TagDto> tags;

        private ICalendarComponent component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private ErrorHandler errorHandler;
        private bool isSelected = false;
        private LightPatientDto patient;
        private TagDto selectedTag;
        private string subject;

        #endregion Fields

        #region Constructors

        public DateRangeViewModel(DateTime start, DateTime end, IPluginHost host)
            : base(start, end)
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

            this.errorHandler = new ErrorHandler(this);
            this.Tags = new ObservableCollection<TagDto>();
            PluginContext.Host = host;
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.RemoveCommand = new RelayCommand(() => this.Remove());

            try
            {
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_SearchProcessed);
                this.Tags.Refill(tags);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_CantFindTags);
            }
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public ICalendarComponent Component
        {
            get { return component; }
        }

        public IPluginHost Host
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged(() => this.IsSelected);
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILog Logger
        {
            get { return this.errorHandler.Logger; }
        }

        public LightPatientDto Patient
        {
            get { return this.patient; }
            set
            {
                this.patient = value;

                this.subject = (value != null)
                    ? value.DisplayedName
                    : string.Empty;

                this.OnPropertyChanged(() => Patient);
            }
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        public string Subject
        {
            get { return this.subject; }
            set
            {
                this.subject = value;
                this.OnPropertyChanged(() => Subject);
            }
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        public string TimeToDisplay
        {
            get
            {
                var dateFrom = this.StartTime.ToShortDateString();
                var dateTo = (this.StartTime.Date == this.EndTime.Date)
                    ? string.Empty
                    : this.EndTime.ToShortDateString();

                var timeFrom = this.StartTime.ToString("HH:mm");
                var timeTo = this.EndTime.ToString("HH:mm");

                return string.Format(Messages.Title_FromTo, dateFrom, timeFrom, dateTo, timeTo);
            }
        }

        #endregion Properties

        #region Methods

        public static void RefreshTags()
        {
            var c = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
            IList<TagDto> result;
            using (c.UnitOfWork)
            {
                result = c.FindTags(TagCategory.Appointment);
            }
            tags = result;
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void HandleError(Exception ex)
        {
            this.errorHandler.HandleError(ex);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleError(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleError(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleErrorSilently(Exception ex, string format, params object[] args)
        {
            this.HandleErrorSilently(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarning(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleWarning(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarningSilently(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleWarningSilently(ex, format, args);
        }

        private void Add()
        {
            var dr = MessageBox.Show(Messages.Msg_AppointmentAskAdd
                , Messages.Title_Question
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question);

            if (dr == MessageBoxResult.No) return;

            try
            {
                using (this.Component.UnitOfWork)
                {
                    var appointment = Mapper.Map<DateRangeViewModel, AppointmentDto>(this);
                    this.Component.Create(appointment, patient);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_AppointmentAdded);
                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_DuringAppointmentAdd);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Subject);
        }

        private void Remove()
        {
            var dr = MessageBox.Show(Messages.Msg_AppointmentAskRemove
                , Messages.Title_Question
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question);

            if (dr == MessageBoxResult.No) return;

            try
            {
                using (this.Component.UnitOfWork)
                {
                    var appointment = Mapper.Map<DateRangeViewModel, AppointmentDto>(this);
                    this.Component.Remove(appointment, this.Patient);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_AppointmentRemoved);
                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_DuringAppointmentAdd);
            }
        }

        #endregion Methods
    }
}