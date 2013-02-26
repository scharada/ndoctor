namespace Probel.NDoctor.Plugins.PatientOverview.ViewModel
{
    using System;
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
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Translations;

    internal abstract class InsertionViewModel : BaseViewModel
    {
        #region Fields

        protected IPatientDataComponent component;

        #endregion Fields

        #region Constructors

        public InsertionViewModel()
        {
            this.HasInsertedItem = false;
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());

            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
            }
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public bool HasInsertedItem
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        protected abstract bool CanAdd();

        protected abstract void Insert();

        private void Add()
        {
            try
            {
                this.Insert();
                PluginContext.Host.WriteStatus(StatusType.Info, BaseText.InsertDone);
                this.HasInsertedItem = true;
            }
            catch (ExistingItemException ex) { this.Handle.Warning(ex, ex.Message); }
            catch (Exception ex) { this.Handle.Error(ex); }
            finally { this.Close(); }
        }

        #endregion Methods
    }
}