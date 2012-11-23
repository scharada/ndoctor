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
    using System.Linq;


    using log4net;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;

    //Just for the refacoring proud: this god object had 1027 lines of code!
    public abstract class BaseComponent : IBaseComponent, IDisposable
    {
        #region Constructors

        public BaseComponent(ISession session)
            : this()
        {
            this.Session = session;
        }

        public BaseComponent()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is session open.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is session open; otherwise, <c>false</c>.
        /// </value>
        public bool IsSessionOpen
        {
            [InspectionIgnored]
            get
            {
                if (this.Session == null) return false;
                return this.Session.IsOpen;
            }
        }

        /// <summary>
        /// Gets or sets the nHibernate session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public ISession Session
        {
            get;
            set;
        }

        protected ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [InspectionIgnored]
        public void Dispose()
        {
            this.Logger.DebugFormat("\t[{0}] Closing session", this.GetType().Name);
            this.CloseSession();
        }

        /// <summary>
        /// Check if the current session can be used to query the database
        /// </summary>
        /// <exception cref="DalSessionException">Is thrown when the session is not configured correctly</exception>
        internal void CheckSession()
        {
            Assert.IsNotNull(this.Session, "Session");
            if (!Session.IsOpen) throw new SessionNotOpenedException();
        }

        /// <summary>
        /// Finds the state of the database. If there's no state, a fresh one is created and returned
        /// </summary>
        /// <returns>The state of the database</returns>
        internal DatabaseState GetDatabaseState()
        {
            var state = (from i in this.Session.Query<DatabaseState>()
                         select i).FirstOrDefault();
            if (state == null)
            {
                state = new DatabaseState();
                this.Session.Save(state);
            }

            return state;
        }

        private void CloseSession()
        {
            if (this.Session != null && Session.IsOpen)
            {
                this.Session.Flush();
                this.Session.Close();
            }
            else throw new SessionNotOpenedException();
        }

        #endregion Methods
    }
}