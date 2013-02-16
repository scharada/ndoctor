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
    using System.Collections.Generic;

    using Castle.DynamicProxy;

    using log4net;

    using Probel.NDoctor.Domain.Components.AuthorisationPolicies;
    using Probel.NDoctor.Domain.Components.Interceptors;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Remote;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Remote;

    using StructureMap;

    /// <summary>
    /// Give an instance of a component and add dynamic interceptors to the call of every methods
    /// of the components
    /// </summary>
    public class ComponentFactory
    {
        #region Fields

        /// <summary>
        /// First instanciation into the static ctor because it needs the StructureMap configuration before.
        /// </summary>
        private static readonly AuthorisationInterceptor AuthorisationInterceptor;
        private static readonly ProxyGenerator Generator = new ProxyGenerator(new PersistentProxyBuilder());

        private readonly bool BenchmarkEnabled = false;
        private readonly uint ExecutionTimeThreshold;
        private readonly bool IsUnderTest = false;
        private readonly ILog Logger = LogManager.GetLogger(typeof(ComponentFactory));

        #endregion Fields

        #region Constructors

        static ComponentFactory()
        {
            ObjectFactory.Configure(x =>
            {

                //Administration plugin
                x.For<IAdministrationComponent>().Add<AdministrationComponent>();
                x.SelectConstructor<AdministrationComponent>(() => new AdministrationComponent());

                //BmiRecord plugin
                x.For<IBmiComponent>().Add<BmiComponent>();
                x.SelectConstructor<BmiComponent>(() => new BmiComponent());

                //DbConvert plugin
                x.For<IImportComponent>().Add<ImportComponent>();
                x.SelectConstructor<ImportComponent>(() => new ImportComponent());

                //Debug plugin
                x.For<ISqlComponent>().Add<SqlComponent>();
                x.SelectConstructor<SqlComponent>(() => new SqlComponent()); ;

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

                //Authorisation manager
                x.For<IAuthorisationComponent>().Add<AuthorisationComponent>();
                x.SelectConstructor<AuthorisationComponent>(() => new AuthorisationComponent());

                //Application statistics manager
                x.For<IApplicationStatisticsComponent>().Add<ApplicationStatisticsComponent>();
                x.SelectConstructor<ApplicationStatisticsComponent>(() => new ApplicationStatisticsComponent());

                //Data statistics manager
                x.For<IDataStatisticsComponent>().Add<DataStatisticsComponent>();
                x.SelectConstructor<DataStatisticsComponent>(() => new DataStatisticsComponent());

                //Database settings
                x.For<IDbSettingsComponent>().Add<DbSettingsComponent>();
                x.SelectConstructor<DbSettingsComponent>(() => new DbSettingsComponent());

                //Authorisation policies
                x.For<IAuthorisationPolicy>().Add<AuthorisationPolicy>();

                //Check remotly for new versions
                x.For<IVersionNotifyer>().Add<VersionNotifyer>();
                x.SelectConstructor<VersionNotifyer>(() => new RemoteFactory().NewVersionNotifyer());

                //Data manager
                x.For<IRescueToolsComponent>().Add<RescueToolsComponent>();
                x.SelectConstructor<RescueToolsComponent>(() => new RescueToolsComponent());
            });

            AuthorisationInterceptor = new AuthorisationInterceptor();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFactory"/> class.
        /// </summary>
        /// <param name="benchmarkEnabled">if set to <c>true</c> component loggin is enabled.</param>
        public ComponentFactory(bool benchmarkEnabled, uint executionTimeThreshold)
            : this(benchmarkEnabled, executionTimeThreshold, false)
        {
        }

        private ComponentFactory(bool benchmarkEnabled, uint executionTimeThreshold, bool isUnderTest)
        {
            this.BenchmarkEnabled = benchmarkEnabled;
            this.ExecutionTimeThreshold = executionTimeThreshold;
            this.IsUnderTest = isUnderTest;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets a factory ready for test. That's only the Authorisation proxy will be hooked to the instance.
        /// </summary>
        public static ComponentFactory TestInstance(bool benchmarkEnabled = false)
        {
            return new ComponentFactory(benchmarkEnabled, int.MaxValue, true);
        }

        /// <summary>
        /// Connects the specified user into the application.
        /// </summary>
        /// <param name="user">The user.</param>
        public void ConnectUser(LightUserDto user)
        {
            AuthorisationInterceptor.User = user;
        }

        /// <summary>
        /// Gets hte configured instance for the specified interface.
        /// </summary>
        /// <typeparam name="T">The IoC Container will search for an instance of that specified interface</typeparam>
        /// <returns>An instance that imlements the specified interface</returns>
        public T GetInstance<T>()
            where T : class
        {
            try
            {
                T instance = ObjectFactory.GetInstance<T>();

                if (instance is BaseComponent)
                {
                    return Generator.CreateInterfaceProxyWithTarget<T>(instance, this.GetInterceptors());
                }
                else { return instance; }
            }
            catch (Exception ex)
            {
                this.Logger.Warn("An error occured when instanciating a component", ex);
                throw new ComponentException(ex);
            }
        }

        /// <summary>
        /// Gets hte configured instance that will be used for unit tests for the specified interface.
        /// This instance will have only one dynamic proxy of type <see cref="TransactionInterceptor"/>.
        /// Using this 
        /// </summary>
        /// <typeparam name="T">The IoC Container will search for an instance of that specified interface</typeparam>
        /// <returns>An instance that imlements the specified interface</returns>
        private IInterceptor[] GetInterceptors()
        {
            var interceptors = new List<IInterceptor>();

            if (this.BenchmarkEnabled) { interceptors.Add(new BenchmarkInterceptor(this.ExecutionTimeThreshold)); }
            if (!this.IsUnderTest)
            {
                interceptors.Add(new TransactionInterceptor());
                interceptors.Add(new LogInterceptor());
            }
            interceptors.Add(AuthorisationInterceptor);

            return interceptors.ToArray();
        }

        #endregion Methods
    }
}