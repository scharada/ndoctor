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

    public class RibbonButtonData : RibbonControlData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButtonData"/> class.
        /// </summary>
        public RibbonButtonData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        public RibbonButtonData(string label, string uriImage)
            : this(label, uriImage, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="command">The command.</param>
        public RibbonButtonData(string label, ICommand command)
            : this(label, null, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonButtonData(string label, string uriImage, ICommand command)
        {
            this.Label = label;
            this.Command = command;

            if (!string.IsNullOrEmpty(uriImage))
            {
                this.LargeImage
                    = this.SmallImage
                    = new Uri(uriImage, UriKind.RelativeOrAbsolute);
            }
        }

        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return string.Format("Menu: {0}", this.Label);
        }

        #endregion Methods
    }
}