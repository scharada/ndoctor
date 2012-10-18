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

namespace Probel.NDoctor.Plugins.FamilyManager.Helpers
{
    using System;
    using System.Collections.Generic;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.View;
    using Probel.NDoctor.Plugins.FamilyManager.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.Services.Messaging;

    internal class ViewService
    {
        #region Fields
        private readonly ErrorHandler Handle;
        private IFamilyComponent Component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();

        #endregion Fields

        #region Constructors

        public ViewService()
        {
            PluginContext.Host.NewUserConnected += (sendere, e) => this.Component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();
            this.Handle = new ErrorHandler(this);
        }

        #endregion Constructors

        #region Methods

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }

        public RemoveFamilyViewModel GetViewModel(RemoveFamilyView view)
        {
            return (RemoveFamilyViewModel)view.DataContext;
        }

        public RemoveFamilyView NewRemoveFamilyView()
        {
            var view = new RemoveFamilyView();

            try
            {
                var family = this.Component.GetAllFamilyMembers(PluginContext.Host.SelectedPatient);

                var mappedCollection = AutoMapper.Mapper.Map<IEnumerable<LightPatientDto>, IEnumerable<LightPatientViewModel>>(family);

                foreach (var mapped in mappedCollection)
                {
                    mapped.SessionPatient = PluginContext.Host.SelectedPatient;
                }
                this.GetViewModel(view).FamilyMembers.Refill(mappedCollection);
            }
            catch (Exception ex) { this.Handle.Error(ex); }

            return view;
        }

        #endregion Methods
    }
}