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

    public class PathologyBoxViewModel
    {
        #region Fields

        private IPatientDataComponent component;

        #endregion Fields

        #region Constructors

        public PathologyBoxViewModel()
        {
            this.Tags = new ObservableCollection<TagDto>();
            component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
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
            IList<TagDto> result;
            using (this.component.UnitOfWork)
            {
                result = this.component.FindTags(TagCategory.Pathology);
            }
            this.Tags.Refill(result);
        }

        #endregion Methods
    }
}