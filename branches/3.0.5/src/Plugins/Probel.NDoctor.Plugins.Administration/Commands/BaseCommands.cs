namespace Probel.NDoctor.Plugins.Administration.Commands
{
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Plugins;
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
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal abstract class BaseCommands
    {
        #region Fields

        protected readonly IErrorHandler Handle;
        protected readonly WorkbenchViewModel ViewModel;

        #endregion Fields

        #region Constructors

        public BaseCommands(WorkbenchViewModel viewModel, IErrorHandler errorHandler)
        {
            this.ViewModel = viewModel;
            this.Handle = errorHandler;
            this.Component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
        }

        #endregion Constructors

        #region Properties

        public IAdministrationComponent Component
        {
            get; set;
        }

        #endregion Properties
    }
}