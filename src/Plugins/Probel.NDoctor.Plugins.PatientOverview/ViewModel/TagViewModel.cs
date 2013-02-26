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

namespace Probel.NDoctor.Plugins.PatientOverview.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox.Navigation;
    using Probel.NDoctor.View.Toolbox.Threads;

    internal class TagViewModel : RequestCloseViewModel
    {
        #region Fields

        protected readonly IPatientDataComponent Component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
        protected readonly IErrorHandler Handle;

        private readonly ICommand closeViewCommand;

        #endregion Fields

        #region Constructors

        public TagViewModel()
        {
            this.Handle = new ErrorHandlerFactory().New(this);
            this.closeViewCommand = new RelayCommand(() => this.CloseView(), () => this.CanCloseView());
        }

        #endregion Constructors

        #region Properties

        public ICommand CloseViewCommand
        {
            get { return this.closeViewCommand; }
        }

        public bool IsModified
        {
            get;
            protected set;
        }

        #endregion Properties

        #region Methods

        private bool CanCloseView()
        {
            return true;
        }

        private void CloseView()
        {
            this.IsModified = false;
            this.Close();
        }

        #endregion Methods
    }
}