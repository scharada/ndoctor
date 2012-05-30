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

namespace Probel.NDoctor.View.Core.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.Helpers.WPF;

    public class DrugBoxViewModel
    {
        #region Fields

        private IAdministrationComponent component;

        #endregion Fields

        #region Constructors

        public DrugBoxViewModel()
        {
            this.Tags = new ObservableCollection<TagDto>();
            if (!Designer.IsDesignMode)
            {
                component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            }
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            IList<TagDto> result = new List<TagDto>();
            using (this.component.UnitOfWork)
            {
                result = component.FindTags(TagCategory.Drug);
            }
            this.Tags.Refill(result);
        }

        #endregion Methods
    }
}