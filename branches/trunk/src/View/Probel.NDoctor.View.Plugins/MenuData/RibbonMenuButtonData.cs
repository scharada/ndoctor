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
namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System.Collections.ObjectModel;

    public class RibbonMenuButtonData : RibbonControlData
    {
        #region Fields

        private ObservableCollection<RibbonControlData> controlDataCollection = new ObservableCollection<RibbonControlData>();
        private bool isApplicationMenu;
        private int nestingDepth;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuButtonData"/> class.
        /// </summary>
        public RibbonMenuButtonData()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuButtonData"/> class.
        /// </summary>
        /// <param name="isApplicationMenu">if set to <c>true</c> [is application menu].</param>
        public RibbonMenuButtonData(bool isApplicationMenu)
        {
            this.IsApplicationMenu = isApplicationMenu;
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<RibbonControlData> ControlDataCollection
        {
            get { return this.controlDataCollection; }
        }

        public bool IsApplicationMenu
        {
            get { return this.isApplicationMenu; }
            set
            {
                this.isApplicationMenu = value;
                this.OnPropertyChanged("IsApplicationMenu");
            }
        }

        public int NestingDepth
        {
            get { return this.nestingDepth; }
            set
            {
                this.nestingDepth = value;
                this.OnPropertyChanged("NestingDepth");
            }
        }

        #endregion Properties
    }
}