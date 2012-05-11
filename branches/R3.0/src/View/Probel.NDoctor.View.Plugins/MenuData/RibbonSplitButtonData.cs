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
    using System;
    using System.Windows.Input;

    public class RibbonSplitButtonData : RibbonMenuButtonData
    {
        #region Fields

        private RibbonButtonData dropDownButtonData;
        private bool isCheckable;
        private bool isChecked;
        private bool isDropDownOpen;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        public RibbonSplitButtonData()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="isApplicationMenu">if set to <c>true</c> [is application menu].</param>
        public RibbonSplitButtonData(bool isApplicationMenu)
            : base(isApplicationMenu)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        public RibbonSplitButtonData(string label, string uriImage)
            : this(label, uriImage, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="command">The command.</param>
        public RibbonSplitButtonData(string label, ICommand command)
            : this(label, null, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonSplitButtonData(string label, string uriImage, ICommand command)
            : base(label, uriImage, command)
        {
        }

        #endregion Constructors

        #region Properties

        public RibbonButtonData DropDownButtonData
        {
            get
            {
                if (dropDownButtonData == null)
                {
                    dropDownButtonData = new RibbonButtonData();
                }

                return dropDownButtonData;
            }
        }

        public bool IsCheckable
        {
            get { return isCheckable; }
            set
            {
                if (isCheckable != value)
                {
                    isCheckable = value;
                    this.OnPropertyChanged(() => IsCheckable);
                }
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    this.OnPropertyChanged(() => IsChecked);
                }
            }
        }

        public bool IsDropDownOpen
        {
            get { return this.isDropDownOpen; }
            set
            {
                this.isDropDownOpen = value;
                this.OnPropertyChanged(() => IsDropDownOpen);
            }
        }

        #endregion Properties
    }
}