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

namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This plugin has a static workbench. That's it only can exist one instance of the workbench
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StaticViewPlugin<T> : Plugin
        where T : new()
    {
        #region Fields

        private T view;

        #endregion Fields

        #region Constructors

        public StaticViewPlugin(Version version)
            : base(version)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the static workbench.
        /// </summary>
        protected T View
        {
            get
            {
                if (view == null) view = new T();
                return view;
            }
        }

        #endregion Properties
    }
}