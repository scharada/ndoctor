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
namespace Probel.NDoctor.Domain.Test.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using NHibernate;

    public static class DbBuilder
    {
        #region Methods

        public static void Create(ISession session)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Domain.Test.InsertUsers.sql");
            if (stream == null) throw new NullReferenceException("The embedded script to create the database can't be loaded.");

            string sql;
            using (var reader = new StreamReader(stream))
            {
                sql = reader.ReadToEnd();
            }

            var regex = new Regex(";", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] lines = regex.Split(sql);

            using (var tx = session.BeginTransaction())
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    var query = session.CreateSQLQuery(line);
                    query.ExecuteUpdate();
                }
                tx.Commit();
            }
        }

        #endregion Methods
    }
}