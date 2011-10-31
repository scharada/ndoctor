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

namespace Probel.NDoctor.Domain.DTO.Components
{
    using Probel.NDoctor.Domain.DTO.Exceptions;

    using StructureMap;

    /// <summary>
    /// Gives instances of on the components.
    /// </summary>
    public static class ComponentFactory
    {
        #region Constructors

        /// <summary>
        /// Checks whether StructureMap was configured in the DAL library.
        /// </summary>
        static ComponentFactory()
        {
            if (!ObjectFactory.Container.Model.HasImplementationsFor<IAdministrationComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IBmiComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<ICalendarComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IDebugComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IFamilyComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IMedicalRecordComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IPathologyComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IPatientDataComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IPatientSessionComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IPictureComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IPrescriptionComponent>()
             || !ObjectFactory.Container.Model.HasImplementationsFor<IUserSessionComponent>())
            {
                throw new DALNotConfiguredException();
            }
        }

        #endregion Constructors

        #region Properties

        public static IAdministrationComponent AdministrationComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IAdministrationComponent>();
            }
        }

        public static IBmiComponent BmiComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IBmiComponent>();
            }
        }

        public static ICalendarComponent CalendarComponent
        {
            get
            {
                return ObjectFactory.GetInstance<ICalendarComponent>();
            }
        }

        public static IDebugComponent DebugComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IDebugComponent>();
            }
        }

        public static IFamilyComponent FamilyComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IFamilyComponent>();
            }
        }

        public static IMedicalRecordComponent MedicalRecordComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IMedicalRecordComponent>();
            }
        }

        public static IPathologyComponent PathologyComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IPathologyComponent>();
            }
        }

        public static IPatientDataComponent PatientDataComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IPatientDataComponent>();
            }
        }

        public static IPatientSessionComponent PatientSessionComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IPatientSessionComponent>();
            }
        }

        public static IPictureComponent PictureComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IPictureComponent>();
            }
        }

        public static IPrescriptionComponent PrescriptionComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IPrescriptionComponent>();
            }
        }

        public static IUserSessionComponent UserSessionComponent
        {
            get
            {
                return ObjectFactory.GetInstance<IUserSessionComponent>();
            }
        }

        #endregion Properties
    }
}