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
    using System.Windows.Input;

    public class RibbonToggleButtonData : RibbonControlData
    {
        #region Fields

        private bool isChecked;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonToggleButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonToggleButtonData(string label, string uriImage, ICommand command)
            : base(label, uriImage, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonToggleButtonData"/> class.
        /// </summary>
        public RibbonToggleButtonData()
            : base()
        {
        }

        #endregion Constructors

        #region Properties

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                this.OnPropertyChanged(() => IsChecked);
            }
        }

        #endregion Properties
    }
}