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

namespace Probel.NDoctor.Domain.DAL.Cfg
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using log4net;

    using Probel.NDoctor.Domain.DAL.Components;

    public class DatabaseCreator
    {
        #region Fields

        public static readonly string InitialDataFilename = Path.Combine(Path.GetTempPath(), "NDoctorTest.db");

        private readonly ILog Logger = LogManager.GetLogger(typeof(DatabaseCreator));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the reference database exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if database exists; otherwise, <c>false</c>.
        /// </value>
        public bool DatabaseExists
        {
            get
            {
                return File.Exists(InitialDataFilename);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a reference database with test data in the temps directory. It'll remove any previous existing
        /// database
        /// </summary>
        public void Create()
        {
            if (File.Exists(InitialDataFilename)) File.Delete(InitialDataFilename);

            new Database().ConfigureUsingFile(InitialDataFilename, true);
            var sql = this.GetScript();
            var component = new DebugComponent();

            using (component.UnitOfWork) { component.ExecuteSql(sql); }
        }

        private string GetScript()
        {
            this.Logger.Debug("Create default values into the database");
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Domain.DAL.InsertUsers.sql");
            if (stream == null) throw new NullReferenceException("The embedded script to create the database can't be loaded or doesn't exist.");

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion Methods
    }
}