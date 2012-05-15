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
namespace Probel.Helpers.WPF.Calendar.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Probel.Helpers.WPF.Calendar.Model;

    public static class Filters
    {
        #region Methods

        public static IEnumerable<Appointment> ByDate(this IEnumerable<Appointment> appointments, DateTime date)
        {
            if (appointments == null) return new AppointmentCollection();
            else
            {
                var app = from a in appointments
                          where a.StartTime.Date == date.Date
                          select a;
                return app;
            }
        }

        #endregion Methods
    }
}