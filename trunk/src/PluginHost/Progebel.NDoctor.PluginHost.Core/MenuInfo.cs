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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.PluginHost.Core
{
    using System;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Abstraction of a menu in the GUI
    /// </summary>
    public class MenuInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the command the click on this button will trigger.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group under which this menu will be displayed.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon displayed for this menu.
        /// If this menu doesn't have images to be displayed
        /// set this property to null
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public Bitmap Icon
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return Imaging.CreateBitmapSourceFromHBitmap(this.Icon.GetHbitmap()
                                                         , IntPtr.Zero
                                                         , Int32Rect.Empty
                                                         , BitmapSizeOptions.FromEmptyOptions());
            }
        }

        /// <summary>
        /// Gets or sets the name displayed for this menu.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent of this menu. That's this menu
        /// will be displayed under this menu.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Menus Parent
        {
            get;
            set;
        }

        #endregion Properties
    }
}