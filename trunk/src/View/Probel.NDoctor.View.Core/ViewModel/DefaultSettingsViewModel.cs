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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins;

    internal class DefaultSettingsViewModel : PluginSettingsViewModel
    {
        #region Fields

        public readonly string configuredLanguage = Settings.Default.Language;

        private DateTime end;
        private Tuple<string, SearchOn> selectedSearchType;
        private bool showRestart;
        private DateTime start;

        #endregion Fields

        #region Constructors

        public DefaultSettingsViewModel()
        {
            this.SupportedLanguages = new ObservableCollection<string>();
            this.SupportedLanguages.Add(Languages.French);
            this.SupportedLanguages.Add(Languages.English);

            this.ChangeLanguageCommand = new RelayCommand(() => this.ShowRestart = (this.SelectedLanguage != configuredLanguage));

            this.FeedSearchTypes();
        }

        #endregion Constructors

        #region Properties

        public bool AutomaticContextMenu
        {
            get { return Settings.Default.AutomaticContextMenu; }
            set
            {
                Settings.Default.AutomaticContextMenu
                    = PluginContext.Configuration.AutomaticContextMenu
                    = value;
                this.OnPropertyChanged(() => AutomaticContextMenu);
            }
        }

        public ICommand ChangeLanguageCommand
        {
            get;
            private set;
        }

        public bool RemoteStatisticsActivated
        {
            get { return Settings.Default.IsRemoteStatisticsEnabled; }
            set
            {
                Settings.Default.IsRemoteStatisticsEnabled = value;
                this.OnPropertyChanged(() => RemoteStatisticsActivated);
            }
        }

        public ObservableCollection<Tuple<string, SearchOn>> SearchTypes
        {
            get;
            private set;
        }

        public string SelectedLanguage
        {
            get
            {
                return Settings.Default.Language;
            }
            set
            {
                Settings.Default.Language = value;
                this.OnPropertyChanged(() => SelectedLanguage);
            }
        }

        public Tuple<string, SearchOn> SelectedSearchType
        {
            get { return this.selectedSearchType; }
            set
            {
                this.selectedSearchType = value;
                this.OnPropertyChanged(() => SelectedSearchType);
            }
        }

        public bool ShowRestart
        {
            get { return this.showRestart; }
            set
            {
                this.showRestart = value;
                this.OnPropertyChanged(() => ShowRestart);
            }
        }

        public ObservableCollection<string> SupportedLanguages
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected override bool CanSave()
        {
            return true;
        }

        protected override void Save()
        {
            Settings.Default.Language = this.SelectedLanguage;
            Settings.Default.AutomaticContextMenu = this.AutomaticContextMenu;
            Settings.Default.SearchType = this.SelectedSearchType.Item2;

            Settings.Default.Save();

            PluginContext.Configuration.SearchType = Settings.Default.SearchType;
        }

        private void FeedSearchTypes()
        {
            this.SearchTypes = new ObservableCollection<Tuple<string, SearchOn>>();
            foreach (SearchOn item in Enum.GetValues(typeof(SearchOn)))
            {
                SearchTypes.Add(new Tuple<string, SearchOn>(item.Translate(), item));
            }

            this.SelectedSearchType = (from s in this.SearchTypes
                                       where s.Item2 == Settings.Default.SearchType
                                       select s).Single();
        }

        #endregion Methods
    }
}