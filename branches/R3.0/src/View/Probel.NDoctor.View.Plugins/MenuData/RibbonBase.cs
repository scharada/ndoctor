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

namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System;

    using Probel.Helpers.Events;

    public class RibbonBase : ObservableObject
    {
        #region Fields

        private string name = Guid.NewGuid().ToString();
        private int order;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the name of this control. It is used to find controls
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public int Order
        {
            get { return this.order; }
            set
            {
                this.order = value;
                this.OnPropertyChanged("Order");
            }
        }

        #endregion Properties
    }
}