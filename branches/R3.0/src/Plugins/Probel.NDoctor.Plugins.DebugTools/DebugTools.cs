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

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;

    [Export(typeof(IPlugin))]
    public class DebugTools : Plugin
    {
        #region Fields

        private SQLComponent component = new SQLComponent();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugTools"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public DebugTools([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("1.0.0.0", ValidationMode.Minimum);
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
            this.FillDefaultDatabase();
            this.LoadUserForDebug();
        }

        private void FillDefaultDatabase()
        {
            bool isDataBaseEmpty = false;
            using (this.component.UnitOfWork) { isDataBaseEmpty = this.component.IsDatabaseEmpty(); }

            if (isDataBaseEmpty)
            {
                this.Logger.Debug("Create default values into the database");
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.DebugTools.InsertUsers.sql");
                if (stream == null) throw new NullReferenceException("The embedded script to create the database can't be loaded or doesn't exist.");

                string sql;

                using (var reader = new StreamReader(stream, Encoding.UTF8)) { sql = reader.ReadToEnd(); }
                using (this.component.UnitOfWork) { this.component.ExecuteSql(sql); }
            }
            else
            {
                this.Logger.Warn("The database is not empty. It won't create test data.");
                return;
            }
        }

        private void LoadUserForDebug()
        {
            PluginContext.Host.Invoke(() =>
            {
                using (this.component.UnitOfWork)
                {
                    var patients = this.component.FindPatientsByNameLight("Robris", SearchOn.LastName);
                    if (patients.Count == 0) return;

                    PluginContext.Host.SelectedPatient = patients[0];

                    this.Logger.DebugFormat("Default patient '{0} {1}' loaded for debug"
                        , patients[0].FirstName
                        , patients[0].LastName);
                }
            });
        }

        #endregion Methods
    }
}