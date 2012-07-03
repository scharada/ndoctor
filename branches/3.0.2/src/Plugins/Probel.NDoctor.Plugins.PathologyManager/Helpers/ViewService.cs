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

namespace Probel.NDoctor.Plugins.PathologyManager.Helpers
{
    using System;
    using System.Collections.Generic;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.View;
    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class ViewService
    {
        #region Fields

        private IPathologyComponent component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();

        #endregion Fields

        #region Constructors

        public ViewService()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
        }

        #endregion Constructors

        #region Methods

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }

        public AddPathologyView NewAddPathologyView()
        {
            var view = new AddPathologyView();
            this.GetViewModel(view).Refresh();

            return view;
        }

        private AddPathologyViewModel GetViewModel(AddPathologyView view)
        {
            return (AddPathologyViewModel)view.DataContext;
        }

        #endregion Methods
    }
}