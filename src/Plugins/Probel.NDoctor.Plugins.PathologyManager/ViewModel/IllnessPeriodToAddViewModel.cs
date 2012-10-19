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
namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Helpers;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class IllnessPeriodToAddViewModel : IllnessPeriodDto
    {
        #region Fields

        private readonly IErrorHandler Handle;

        private IPathologyComponent component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
        private bool isSelected;

        #endregion Fields

        #region Constructors

        public IllnessPeriodToAddViewModel()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
            this.Handle = new ErrorHandlerFactory().New(this);
            this.Start
                = this.End
                = DateTime.Today;
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string BtnAdd
        {
            get { return Messages.Title_Add; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected in the ListView.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged(() => IsSelected);
            }
        }

        public string TitleIllFrom
        {
            get { return Messages.Title_IllFrom; }
        }

        public string TitleTo
        {
            get { return Messages.Title_To; }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            Assert.IsNotNull(PluginContext.Host, "PluginContext.Host");

            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskAddNewPeriod, Messages.Title_Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                this.component.Create(this, PluginContext.Host.SelectedPatient);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PeriodAdded);
                Notifyer.OnItemChanged(this);
                InnerWindow.Close();
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_FailAddIllnessPeriod);
            }
        }

        private bool CanAdd()
        {
            return (this.End > this.Start);
        }

        #endregion Methods
    }
}