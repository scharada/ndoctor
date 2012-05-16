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
    using System;

    using Castle.DynamicProxy;

    using log4net;

    using Probel.NDoctor.Domain.Components.Interceptors;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    using StructureMap;

    /// <summary>
    /// Give an instance of a component and add dynamic interceptors to the call of every methods
    /// of the components
    /// </summary>
    public class ComponentFactory
    {
        #region Fields

        private static readonly ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());

        private ILog logger = LogManager.GetLogger(typeof(ComponentFactory));
        private LightUserDto user;

        #endregion Fields

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

                //Patient session plugin
                x.For<IPatientSessionComponent>().Add<PatientSessionComponent>();
                x.SelectConstructor<PatientSessionComponent>(() => new PatientSessionComponent());

                //Picture manager plugin
                x.For<IPictureComponent>().Add<PictureComponent>();
                x.SelectConstructor<PictureComponent>(() => new PictureComponent());

                //Prescription manager plugin
                x.For<IPrescriptionComponent>().Add<PrescriptionComponent>();
                x.SelectConstructor<PrescriptionComponent>(() => new PrescriptionComponent());

                //User session manager
                x.For<IUserSessionComponent>().Add<UserSessionComponent>();
                x.SelectConstructor<UserSessionComponent>(() => new UserSessionComponent());
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFactory"/> class.
        /// </summary>
        /// <param name="user">The user connected into nDoctor.</param>
        public ComponentFactory(LightUserDto user)
        {
            this.user = user;
        }

        #endregion Constructors

        #region Methods

        public T GetInstance<T>()
            where T : class
        {
            try
            {
                T instance = ObjectFactory.GetInstance<T>();

                if (instance is BaseComponent)
                {
                    return generator.CreateInterfaceProxyWithTarget<T>(instance
                        , new CheckerInterceptor()
                        , new LogInterceptor());
                }
                else { return instance; }
            }
            catch (Exception ex)
            {
                this.logger.Warn("An error occured when instanciating a component", ex);
                throw;
            }
        }

        #endregion Methods
    }
}