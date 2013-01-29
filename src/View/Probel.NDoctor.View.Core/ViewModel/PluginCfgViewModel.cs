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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.IO;

    using AutoMapper;


    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.View.Plugins.Cfg;

    internal class PluginCfgViewModel : PluginSettingsViewModel
    {
        #region Fields

        private readonly string FileName;

        private PluginConfigurationDto selectedConfiguration;

        #endregion Fields

        #region Constructors

        public PluginCfgViewModel()
        {
            try
            {
                this.Configurations = new ObservableCollection<PluginConfigurationDto>();
                var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(appdata, @"Probel\nDoctor\Plugins.config");

                this.FileName = (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["pluginSettings"]))
                     ? path
                     : ConfigurationManager.AppSettings["appSettings"];

                this.Load();
            }
            catch (Exception ex) { this.Logger.Error("An error occured when instanciating 'PluginCfgViewModel'", ex); }
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<PluginConfigurationDto> Configurations
        {
            get;
            private set;
        }

        public PluginConfigurationDto SelectedConfiguration
        {
            get { return this.selectedConfiguration; }
            set
            {
                this.selectedConfiguration = value;
                this.OnPropertyChanged(() => SelectedConfiguration);
            }
        }

        private PluginsConfigurationFolder Folder
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        protected override bool CanSave()
        {
            return true;
        }

        protected override void Save()
        {
            try
            {
                var cfg = Mapper.Map<IEnumerable<PluginConfigurationDto>, IEnumerable<PluginConfiguration>>(this.Configurations);
                this.Folder.Clear();
                this.Folder.AddRange(cfg);
                this.Folder.Save(this.FileName);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Load()
        {
            try
            {
                this.Folder = (!File.Exists(FileName))
                    ? PluginsConfigurationFolder.LoadDefault()
                    : PluginsConfigurationFolder.Load(FileName);

                var dto = Mapper.Map<IEnumerable<PluginConfiguration>, IEnumerable<PluginConfigurationDto>>(this.Folder.Values);
                this.Configurations.Refill(dto);

                if (this.Configurations.Count > 0)
                {
                    this.SelectedConfiguration = this.Configurations[0];
                }
            }
            catch (Exception ex) { this.Logger.Error(ex); }
        }

        #endregion Methods
    }
}