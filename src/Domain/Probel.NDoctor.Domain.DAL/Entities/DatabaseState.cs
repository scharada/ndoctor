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
namespace Probel.NDoctor.Domain.DAL.Entities
{
    /// <summary>
    /// Provides a state of the database. This is used to trigger some processing depending of the state.
    /// For instance, nDoctor will know whether or not execute the thumbnail maker by checking this object
    /// </summary>
    internal class DatabaseState : Entity
    {
        #region Constructors

        public DatabaseState()
        {
            this.AreThumbnailsCreated = false;
        }

        #endregion Constructors

        #region Properties

        public virtual bool AreThumbnailsCreated
        {
            get; set;
        }

        #endregion Properties
    }
}