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
namespace Probel.NDoctor.Plugins.DebugTools
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginName, "Debug tools")]
    public class DebugTools : Plugin
    {
        #region Fields

        private readonly PluginSettings Settings = new PluginSettings();

        private ISqlComponent component;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugTools"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public DebugTools()
            : base()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<ISqlComponent>();
            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<ISqlComponent>();
            this.Logger.Warn("Debug plugin is loaded. It shouldn't be used in production!");
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost.
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            if (this.Settings.InjectDefaultData)
            {
                this.Logger.Debug("Injecting default data");
                this.FillDefaultDatabase();
            }
            if (this.Settings.LoadDefaultUser)
            {
                this.Logger.Debug("Loading default user");
                this.LoadPatientForDebug();
            }
            if (this.Settings.DefaultGoogleCalendarConfig)
            {
                this.Logger.Debug("Fill default config for Google Calendar");
                this.FillDefaultCalendarConfig(this.Settings.IsGoogleActivated);
            }
        }

        private void FillDefaultCalendarConfig(bool isActivated)
        {
            try
            {
                var stg = new MeetingManagerSettings();
                stg.IsGoogleCalendarActivated = isActivated;
                stg.Password = "ndoctorndoctor".Encrypt();
                stg.UserName = "ndoctor.development@gmail.com";
                stg.Save();

                this.Logger.Warn("Setup the debug configuration for the Meeting manager");
            }
            catch (Exception ex) { this.Logger.Error("An error occured while creating the debug configuration for the Meeting Manager", ex); }
        }

        private void FillDefaultDatabase()
        {
            var isDataBaseEmpty = this.component.IsDatabaseEmpty();

            if (isDataBaseEmpty)
            {
                this.Logger.Warn("Create default values into the database");
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.DebugTools.InsertUsers.sql");
                if (stream == null) throw new NullReferenceException("The embedded script to create the database can't be loaded or doesn't exist.");

                string sql;

                using (var reader = new StreamReader(stream, Encoding.UTF8)) { sql = reader.ReadToEnd(); }
                this.component.ExecuteSql(sql);
            }
            else
            {
                this.Logger.Warn("The database is not empty. It won't create test data.");
                return;
            }
        }

        private void LoadPatientForDebug()
        {
            PluginContext.Host.Invoke(() =>
            {
                var patients = this.component.GetPatientsByNameLight(this.Settings.DefaultPatient, SearchOn.LastName);
                if (patients.Count == 0) return;

                PluginContext.Host.SelectedPatient = patients[0];

                this.Logger.DebugFormat("Default patient '{0} {1}' loaded for debug"
                    , patients[0].FirstName
                    , patients[0].LastName);
            });
        }

        #endregion Methods
    }
}