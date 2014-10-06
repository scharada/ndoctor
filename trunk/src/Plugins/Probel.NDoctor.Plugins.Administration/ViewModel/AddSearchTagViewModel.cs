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

namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;

    class AddSearchTagViewModel : BaseBoxViewModel<SearchTagDto>
    {
        #region Constructors

        public AddSearchTagViewModel()
        {
            this.BoxItem = new SearchTagDto();
        }

        #endregion Constructors

        #region Methods

        protected override void AddItem()
        {
            if (!this.Component.CheckSearchTagExist(this.BoxItem.Name))
            {
                this.Component.Create(this.BoxItem);
            }
            else { ViewService.MessageBox.Warning(Messages.Msg_WanSearchTagExists); }
        }

        #endregion Methods
    }
}