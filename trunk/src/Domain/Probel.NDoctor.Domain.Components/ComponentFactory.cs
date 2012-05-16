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

namespace Probel.NDoctor.Domain.Components
{
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;

    using StructureMap;

    /// <summary>
    /// Give an instance of a component
    /// </summary>
    public class ComponentFactory
    {
        #region Constructors

        static ComponentFactory()
        {
            ObjectFactory.Configure(x =>
            {
                //Administration plugin
                x.For<IAdministrationComponent>().Add<AdministrationComponent>();
                x.SelectConstructor<IAdministrationComponent>(() => new AdministrationComponent());

                //BmiRecord plugin
                x.For<IBmiComponent>().Add<BmiComponent>();
                x.SelectConstructor<BmiComponent>(() => new BmiComponent());

                //DbConvert plugin
                x.For<IImportComponent>().Add<ImportComponent>();
                x.SelectConstructor<ImportComponent>(() => new ImportComponent());

                //Debug plugin
                x.For<ISqlComponent>().Add<SqlComponent>();

                //Family manager plugin
                x.For<IFamilyComponent>().Add<FamilyComponent>();
                x.SelectConstructor<FamilyComponent>(() => new FamilyComponent());

                //Medical record plugin
                x.For<IMedicalRecordComponent>().Add<MedicalRecordComponent>();
                x.SelectConstructor<MedicalRecordComponent>(() => new MedicalRecordComponent());

                //Meeting manager plugin
                x.For<ICalendarComponent>().Add<CalendarComponent>();
                x.SelectConstructor<CalendarComponent>(() => new CalendarComponent());

                //Pathology plugin
                x.For<IPathologyComponent>().Add<PathologyComponent>();
                x.SelectConstructor<PathologyComponent>(() => new PathologyComponent());

                //Patient data plugin
                x.For<IPatientDataComponent>().Add<PatientDataComponent>();
                x.SelectConstructor<PatientDataComponent>(() => new PatientDataComponent());

            });
        }

        #endregion Constructors

        #region Methods

        public T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        #endregion Methods
    }
}