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

    using NHibernate;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class Script
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Script));

        #endregion Fields

        #region Methods

        public void Execute(ISession session)
        {
            try
            {
                Logger.Info("Start default value insertion's script");

                var administer = new TaskDto(To.Administer) { Name = Messages.Task_Administer, Notes = Messages.Explanation_Administer };
                var metawrite = new TaskDto(To.MetaWrite) { Name = Messages.Task_Metawrite, Notes = Messages.Explanation_Metawrite };
                var read = new TaskDto(To.Read) { Name = Messages.Task_Read, Notes = Messages.Explanation_Read };
                var write = new TaskDto(To.Write) { Name = Messages.Task_Write, Notes = Messages.Explanation_Write };
                var editcalendar = new TaskDto(To.EditCalendar) { Name = Messages.Task_EditCalendar, Notes = Messages.Explanation_EditCalendar };
                CheckTaksNumber(administer, metawrite, read, write, editcalendar);

                var administrator = BuildRole(Messages.Role_Administrator, Messages.Explanation_Administrator
                    , administer, metawrite, read, write, editcalendar);

                var doctor = BuildRole(Messages.Role_Doctor, Messages.Explanation_Doctor
                    , metawrite, read, write, editcalendar);

                var secretary = BuildRole(Messages.Role_Secretary, Messages.Explanation_Secretary
                    , metawrite, read, editcalendar);

                var component = (session == null)
                    ? new AuthorisationComponent()         //Used in production
                    : new AuthorisationComponent(session); //Used in unit test sessions

                component.Create(administer);
                component.Create(metawrite);
                component.Create(read);
                component.Create(write);
                component.Create(editcalendar);

                component.Create(administrator);
                component.Create(doctor);
                component.Create(secretary);

                //Uncomment to create a superadmin as a first user
                //var superadmin = new UserDto(true) { FirstName = "Superadmin", LastName = "Superadmin", AssignedRole = administrator, IsDefault = true };
                //component.Create(superadmin);
                //component.Update(superadmin, "superadmin"); //Set a default password

                component.Create(new TagDto(TagCategory.Prescription) { Name = Messages.Tag_Default_Prescription });
                component.Create(new TagDto(TagCategory.Appointment) { Name = Default.GoogleCalendarTagName });

                Logger.Info("Script is done...");

            }
            catch (Exception ex)
            {
                Logger.Warn("Script failed", ex);
                throw;
            }
        }

        internal void Execute()
        {
            this.Execute(null);
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