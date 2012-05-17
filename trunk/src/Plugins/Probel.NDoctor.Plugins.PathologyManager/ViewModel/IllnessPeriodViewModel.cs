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

    using Probel.Helpers.Events;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class IllnessPeriodViewModel : IllnessPeriodDto
    {
        #region Fields

        private IPathologyComponent component = new ComponentFactory(PluginContext.Host.ConnectedUser, PluginContext.ComponentLogginEnabled).GetInstance<IPathologyComponent>();
        private bool isSelected;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IllnessPeriodViewModel"/> class.
        /// </summary>
        public IllnessPeriodViewModel()
        {
            this.RemoveCommand = new RelayCommand(() =>
            {
                var dr = MessageBox.Show(Messages.Msg_DeleteIllnessPeriod, Messages.Title_Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes)
                {
                    using (this.component.UnitOfWork)
                    {
                        this.component.Remove(this, PluginContext.Host.SelectedPatient);
                    }
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_IllnessRemoved);
                    this.OnRefreshed(State.Removed);
                }
            });
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when this item needs to be refreshed.
        /// </summary>
        public event EventHandler<EventArgs<State>> Refreshed;

        #endregion Events

        #region Properties

        public string DurationAsText
        {
            get
            {
                var span = this.End - this.Start;
                return string.Format(Messages.Title_IllnessDuration, span.TotalDays);
            }
        }

        public string EndAsText
        {
            get { return this.End.ToShortDateString(); }
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

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public string StartAsText
        {
            get { return this.Start.ToShortDateString(); }
        }

        #endregion Properties

        #region Methods

        private void OnRefreshed(State state)
        {
            if (this.Refreshed != null)
                this.Refreshed(this, new EventArgs<State>(state));
        }

        #endregion Methods
    }
}