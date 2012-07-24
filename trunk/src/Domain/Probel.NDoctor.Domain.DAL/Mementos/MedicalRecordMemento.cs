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

namespace Probel.NDoctor.Domain.DAL.Mementos
{
    using System.Linq;

    using AutoMapper;

    using Probel.NDoctor.Domain.DAL.Entities;
    using System;

    public class MedicalRecordMemento : IMemento<MedicalRecord>
    {

        #region Methods

        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SaveState(MedicalRecord item)
        {
            var state = Mapper.Map<MedicalRecord, MedicalRecordState>(item);

            state.LastUpdate = DateTime.Now;
            state.Id = 0;//Clear the ID to mock a new entity

            item.PreviousStates.Add(state);

            item.PreviousStates = (from s in item.PreviousStates
                                   select s)
                                     .OrderByDescending(e => e.LastUpdate)
                                     .Take(10)
                                     .ToList();
        }

        #endregion Methods
    }
}