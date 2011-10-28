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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Conversions;
    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides all the features to query the database about the 
    /// pathologies
    /// </summary>
    public class PathologyComponent : BaseComponent, IPathologyComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified illness period for the specified patient.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="patient">The patient.</param>
        public void Create(IllnessPeriodDto period, LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var illnessPeriod = Mapper.Map<IllnessPeriodDto, IllnessPeriod>(period);
            illnessPeriod.Id = 0;

            entity.IllnessHistory.Add(illnessPeriod);
            this.Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Create(PathologyDto item)
        {
            this.CheckSession();

            var found = (from p in this.Session.Query<Pathology>()
                         where p.Id == item.Id
                            || item.Name.ToLower() == p.Name.ToLower()
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<PathologyDto, Pathology>(item);
            entity.Id = 0;

            this.Session.Save(entity);
        }

        /// <summary>
        /// Gets the illness history for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// The history of illness periods
        /// </returns>
        public IllnessHistoryDto GetIllnessHistory(LightPatientDto patient)
        {
            var illnessHistory = new IllnessHistoryDto() { Patient = patient };
            var entity = this.Session.Get<Patient>(patient.Id);

            var periods = Mapper.Map<IList<IllnessPeriod>, IList<IllnessPeriodDto>>(entity.IllnessHistory);
            illnessHistory.Periods.Refill(periods);
            return illnessHistory;
        }

        /// <summary>
        /// Gets the ilness as a chart. That's a X/Y axes chart when X axes is
        /// </summary>
        /// <param name="patient">The patient used to create the chart.</param>
        /// <returns>
        /// A <see cref="Chart"/> with X axes as pathology name and Y axes
        /// as <see cref="TimeSpan"/> for the duration of the illness.
        /// </returns>
        public Chart<string, double> GetIlnessAsChart(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);

            var result = (from ih in entity.IllnessHistory
                          group ih by new { Id = ih.Pathology.Id, Name = ih.Pathology.Name } into grp
                          select new { Name = grp.Key.Name, TotalDays = grp.Sum(e => e.Duration.TotalDays) });
            var chart = new Chart<string, double>();
            foreach (var item in result)
            {
                chart.AddPoint(item.Name, item.TotalDays);
            }
            return chart;
        }

        /// <summary>
        /// Removes the specified illness period list from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriods">The illness periods.</param>
        /// <param name="patient">The patient.</param>
        public void Remove(IList<IllnessPeriodDto> illnessPeriods, LightPatientDto patient)
        {
            foreach (var illnessPeriod in illnessPeriods)
            {
                this.Remove(illnessPeriod, patient);
            }
        }

        /// <summary>
        /// Removes the specified illness period from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriod">The illness period.</param>
        /// <param name="patient">The patient.</param>
        public void Remove(IllnessPeriodDto illnessPeriod, LightPatientDto patient)
        {
            Assert.IsNotNull(illnessPeriod, "illnessPeriod");
            Assert.IsNotNull(patient, "patient");

            var entity = this.Session.Get<Patient>(patient.Id);

            for (int i = 0; i < entity.IllnessHistory.Count; i++)
            {
                if (entity.IllnessHistory[i].Id == illnessPeriod.Id)
                {
                    entity.IllnessHistory.RemoveAt(i);
                    break;
                }
            }
            this.Session.Update(entity);
        }

        #endregion Methods
    }
}