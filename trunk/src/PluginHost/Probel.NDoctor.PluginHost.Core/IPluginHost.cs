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
namespace Probel.NDoctor.PluginHost.Core
{
    using System.Windows.Controls;

    /// <summary>
    /// Represents the host of all further plugin. This is the API 
    /// the plugins will use to be shown on the GUI
    /// </summary>
    public interface IPluginHost
    {
        #region Methods

        /// <summary>
        /// Sets the specified menu in the contextual menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void SetContextualMenu(string tabName, MenuInfo[] menus);

        /// <summary>
        /// Sets the specified menu in the global menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void SetGlobalMenu(MenuInfo menu);

        /// <summary>
        /// Sets the specified menu in the ribbon menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void SetRibbonMenu(MenuInfo menu);

        /// <summary>
        /// Sets the workbench, that's the main GUI item of the plugin, into the main window.
        /// </summary>
        /// <param name="workbench">The workbench.</param>
        void SetWorkbench(UserControl workbench);

        /// <summary>
        /// Writes the specified status into the status bar.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void WriteStatus(StatusType type, string format, params object[] args);

        #endregion Methods
    }
}