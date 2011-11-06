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
    using System.Data;
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DAL.Cfg;

    public class SQLiteDataLoader
    {
        #region Fields

        private const string ATTACHED_DB = "zxcvbnmInitialData";

        private readonly string InitialDataFilename = DatabaseCreator.InitialDataFilename;

        private static log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SQLiteDataLoader));

        private SQLiteConnection connection;

        #endregion Fields

        #region Constructors

        public SQLiteDataLoader(SQLiteConnection Connection)
        {
            connection = Connection;
        }

        #endregion Constructors

        #region Methods

        public void ImportData()
        {
            DataTable dt = connection.GetSchema(SQLiteMetaDataCollectionNames.Tables);
            var tableNames = (from DataRow R in dt.Rows
                              select (string)R["TABLE_NAME"]).ToArray();
            AttachDatabase();
            foreach (string tableName in tableNames)
            {
                CopyTableData(tableName);
            }
            DetachDatabase();
        }

        private void AttachDatabase()
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = String.Format("ATTACH '{0}' AS {1}", InitialDataFilename, ATTACHED_DB);
            Log.Debug(cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        private void CopyTableData(string TableName)
        {
            int rowsAffected;
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}.{0}", TableName, ATTACHED_DB);
            Log.Debug(cmd.CommandText);
            rowsAffected = cmd.ExecuteNonQuery();
            Log.InfoFormat("{0} {1} rows loaded", rowsAffected, TableName);
        }

        private void DetachDatabase()
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = string.Format("DETACH {0}", ATTACHED_DB);
            Log.Debug(cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        #endregion Methods
    }
}