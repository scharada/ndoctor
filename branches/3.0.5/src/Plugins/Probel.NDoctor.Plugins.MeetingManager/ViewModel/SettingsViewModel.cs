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

namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;

    internal class SettingsViewModel : PluginSettingsViewModel
    {
        #region Fields

        private readonly PluginSettings Settings = new PluginSettings();

        #endregion Fields

        #region Constructors

        public SettingsViewModel()
        {
            this.SlotDurations = new ObservableCollection<Tuple<string, SlotDuration>>();
            this.SlotDurations.Add(new Tuple<string, SlotDuration>(Messages.SlotDuration_OneHour, SlotDuration.OneHour));
            this.SlotDurations.Add(new Tuple<string, SlotDuration>(Messages.SlotDuration_ThirtyMinutes, SlotDuration.ThirtyMinutes));
        }

        #endregion Constructors

        #region Properties

        public string BindedCalendar
        {
            get { return this.Settings.BindedCalendar; }
            set
            {
                this.Settings.BindedCalendar = value;
                this.OnPropertyChanged(() => BindedCalendar);
            }
        }

        public DateTime End
        {
            get { return this.BuildFromString(this.Settings.WorkdayEnd); }
            set
            {
                this.Settings.WorkdayEnd = string.Format("{0}:{1}", value.Hour, value.Minute);
                this.OnPropertyChanged(() => End);
            }
        }

        public bool IsGoogleCalendarActivated
        {
            get { return this.Settings.IsGoogleCalendarActivated; }
            set
            {
                this.Settings.IsGoogleCalendarActivated = value;
                this.OnPropertyChanged(() => IsGoogleCalendarActivated);
            }
        }

        public string Password
        {
            get { return this.Settings.Password; }
            set
            {
                this.Settings.Password = value;
                this.OnPropertyChanged(() => Password);
            }
        }

        public Tuple<string, SlotDuration> SelectedSlot
        {
            get
            {
                var slot = this.Settings.SlotDuration;
                return new Tuple<string, SlotDuration>(Translate(slot), slot);
            }
            set
            {
                this.Settings.SlotDuration = value.Item2;
                this.OnPropertyChanged(() => SelectedSlot);
            }
        }

        public ObservableCollection<Tuple<string, SlotDuration>> SlotDurations
        {
            get;
            private set;
        }

        public DateTime Start
        {
            get { return this.BuildFromString(this.Settings.WorkdayStart); }
            set
            {
                this.Settings.WorkdayStart = string.Format("{0}:{1}", value.Hour, value.Minute);
                this.OnPropertyChanged(() => Start);
            }
        }

        public string UserName
        {
            get { return this.Settings.UserName; }
            set
            {
                this.Settings.UserName = value;
                this.OnPropertyChanged(() => UserName);
            }
        }

        #endregion Properties

        #region Methods

        protected override bool CanSave()
        {
            return this.Start < this.End;
        }

        protected override void Save()
        {
            this.Settings.Save();
        }

        private DateTime BuildFromString(string time)
        {
            var items = time.Split(':');
            var s = DateTime.MinValue.AddHours(Int32.Parse(items[0]));
            s.AddMinutes(Int32.Parse(items[1]));
            return s;
        }

        private string Translate(SlotDuration slotDuration)
        {
            var value = string.Empty;

            switch (slotDuration)
            {
                case SlotDuration.ThirtyMinutes:
                    value = Messages.SlotDuration_ThirtyMinutes;
                    break;
                case SlotDuration.OneHour:
                    value = Messages.SlotDuration_OneHour;
                    break;
                default:
                    Assert.FailOnEnumeration(slotDuration);
                    break;
            }
            return value;
        }

        #endregion Methods
    }
}