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
        private const int MEMENTO_SIZE = 10;
        #region Methods

        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SaveState(MedicalRecord item)
        {
            var state = Mapper.Map<MedicalRecord, MedicalRecordState>(item);

            if (item.PreviousStates.Count >= MEMENTO_SIZE)
            {
                var index = (from c in item.PreviousStates
                             select c).Max(e => e.Counter) + 1;

                /* The modulo calculation is used to find the index where to add the new state in the stack*/
                Mapper.Map<MedicalRecordState, MedicalRecordState>(state, item.PreviousStates[index % MEMENTO_SIZE]);
                item.PreviousStates[index % MEMENTO_SIZE].Counter = index;
                item.PreviousStates[index % MEMENTO_SIZE].LastUpdate = DateTime.Now;

                /* Reset the counter to avoid int overflow. After counting during two loops, everything is reset
                 * such as it was the first time the stack was filled */
                if (index % (MEMENTO_SIZE * 2) == MEMENTO_SIZE - 1)
                {
                    for (int i = 0; i < item.PreviousStates.Count; i++) { item.PreviousStates[i].Counter = i; }
                }
            }
            else
            {
                state.LastUpdate = DateTime.Now;
                state.Counter = item.PreviousStates.Count;
                item.PreviousStates.Add(state);
            }
        }

        #endregion Methods
    }
}