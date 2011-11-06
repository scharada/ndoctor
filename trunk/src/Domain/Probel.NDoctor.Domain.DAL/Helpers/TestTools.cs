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
namespace Probel.NDoctor.Domain.DAL.Helpers
{
    using Probel.NDoctor.Domain.DAL.Components;

    public static class TestTools
    {
        #region Methods

        /// <summary>
        /// Sets the session of the specified component to null.
        /// </summary>
        /// <param name="component">The component.</param>
        public static void NullifySession(BaseComponent component)
        {
            if (component != null) component.SetSession(null);
        }

        #endregion Methods
    }
}