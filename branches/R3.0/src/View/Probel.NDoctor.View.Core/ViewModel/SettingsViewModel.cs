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

namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SettingsViewModel : BaseViewModel
    {
        #region Fields

        private DateTime end;
        private DateTime start;

        #endregion Fields

        #region Constructors

        public SettingsViewModel()
        {
            this.SlotDurations = new ObservableCollection<Tuple<string, SlotDuration>>();
            this.SlotDurations.Add(new Tuple<string, SlotDuration>(Messages.SlotDuration_OneHour, SlotDuration.OneHour));
            this.SlotDurations.Add(new Tuple<string, SlotDuration>(Messages.SlotDuration_ThirtyMinutes, SlotDuration.ThirtyMinutes));

            this.SaveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.SupportedLanguages = new ObservableCollection<string>();
            this.SupportedLanguages.Add(Languages.French);
            this.SupportedLanguages.Add(Languages.English);

            this.Start = this.BuildFromString(Settings.Default.WorkDayStart);
            this.End = this.BuildFromString(Settings.Default.WorkDayEnd);
        }

        #endregion Constructors

        #region Properties

        public DateTime End
        {
            get { return this.end; }
            set
            {
                this.end = value;
                this.OnPropertyChanged("End");
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public string SelectedLanguage
        {
            get { return Settings.Default.Language; }
            set
            {
                Settings.Default.Language = value;
                this.OnPropertyChanged("SelectedLanguage");
            }
        }

        public Tuple<string, SlotDuration> SelectedSlot
        {
            get
            {
                var slot = Settings.Default.SlotDuration;
                return new Tuple<string, SlotDuration>(Translate(slot), slot);
            }
            set
            {
                Settings.Default.SlotDuration = value.Item2;
                this.OnPropertyChanged("SelectedSlot");
            }
        }

        public ObservableCollection<Tuple<string, SlotDuration>> SlotDurations
        {
            get;
            private set;
        }

        public DateTime Start
        {
            get { return this.start; }
            set
            {
                this.start = value;
                this.OnPropertyChanged("Start");
            }
        }

        public ObservableCollection<string> SupportedLanguages
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private DateTime BuildFromString(string time)
        {
            var items = time.Split(':');
            var s = DateTime.MinValue.AddHours(Int32.Parse(items[0]));
            s.AddMinutes(Int32.Parse(items[1]));
            return s;
        }

        private bool CanSave()
        {
            return this.Start < this.End;
        }

        private void Save()
        {
            Settings.Default.WorkDayStart = this.Start.ToString("HH:mm");
            Settings.Default.WorkDayEnd = this.End.ToString("HH:mm");
            Settings.Default.Language = this.SelectedLanguage;

            Settings.Default.Save();
            InnerWindow.Close();
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