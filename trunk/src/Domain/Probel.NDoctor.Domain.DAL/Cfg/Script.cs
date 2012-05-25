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
    using System.Collections.Generic;
    using System.Diagnostics;

    using log4net;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class Script
    {
        #region Fields

        private static readonly IAuthorisationComponent component = new AuthorisationComponent();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Script));

        #endregion Fields

        #region Methods

        public void Execute()
        {
            try
            {
                using (component.UnitOfWork)
                {
                    Logger.Info("Start default value insertion's script");

                    //var everyone = new TaskDto(To.Everyone) { Name = Messages.Task_Everyone, Notes = Messages.Explanation_Everyone };
                    TaskDto administer = new TaskDto(To.Administer) { Name = Messages.Task_Administer, Notes = Messages.Explanation_Administer };
                    TaskDto metawrite = new TaskDto(To.MetaWrite) { Name = Messages.Task_Metawrite, Notes = Messages.Explanation_Metawrite };
                    TaskDto read = new TaskDto(To.Read) { Name = Messages.Task_Read, Notes = Messages.Explanation_Read };
                    TaskDto write = new TaskDto(To.Write) { Name = Messages.Task_Write, Notes = Messages.Explanation_Write };
                    CheckTaksNumber(administer, metawrite, read, write);

                    component.Create(administer);
                    component.Create(metawrite);
                    component.Create(read);
                    component.Create(write);

                    var administrator = BuildRole(Messages.Role_Administrator, Messages.Explanation_Administrator, administer, metawrite, read, write);
                    var doctor = BuildRole(Messages.Role_Doctor, Messages.Explanation_Doctor, metawrite, read, write);
                    var secretary = BuildRole(Messages.Role_Secretary, Messages.Explanation_Secretary, metawrite, read);

                    component.Create(administrator);
                    component.Create(doctor);
                    component.Create(secretary);

                    Logger.Info("Script is done...");
                }
            }
            catch (Exception)
            {
                Logger.Warn("Script failed");
                throw;
            }
        }

        private static RoleDto BuildRole(string name, string description, params TaskDto[] tasks)
        {
            var role = new RoleDto()
            {
                Name = name,
                Description = description,
            };

            foreach (var task in tasks) { role.Tasks.Add(task); }
            return role;
        }

        [Conditional("DEBUG")]
        private static void CheckTaksNumber(params TaskDto[] tasks)
        {
            if (tasks == null)
            {
                throw new NullReferenceException("The tasks to insert into the database are null!");
            }
            if (tasks.Length != To.ToStringArray().Length - 1)
            {
                throw new NotSupportedException(string.Format(
                    "There'r {0} hardcoded tasks in the application but only {1} are inserted into the database!"
                    , To.ToStringArray().Length - 1
                    , tasks.Length));
            }
        }

        private IList<TaskDto> BuildTask(params TaskDto[] tasks)
        {
            List<TaskDto> result = new List<TaskDto>();

            foreach (var task in tasks)
            {
                result.Add(task);
            }
            return result;
        }

        #endregion Methods
    }
}