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
namespace Probel.NDoctor.View.Plugins.Helpers
{
    public interface ITreeNode
    {
        #region Properties

        /// <summary>
        /// Gets or sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        bool IsExpanded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        #endregion Properties
    }
}