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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox.Threads;

    internal class RemoveTagViewModel : TagViewModel
    {
        #region Fields

        private readonly ICommand refreshCommand;
        private readonly ICommand removeCommand;

        #endregion Fields

        #region Constructors

        public RemoveTagViewModel()
        {
            this.removeCommand = new RelayCommand(() => this.Remove(), () => this.CanRemove());
            this.TagsToRemove = new ObservableCollection<SearchTagDto>();
            this.refreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.SearchTags = new ObservableCollection<SearchTagDto>();
        }

        #endregion Constructors

        #region Properties

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return this.removeCommand; }
        }

        public ObservableCollection<SearchTagDto> SearchTags
        {
            get;
            private set;
        }

        public ObservableCollection<SearchTagDto> TagsToRemove
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanRefresh()
        {
            return true;
        }

        private bool CanRemove()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void Refresh()
        {
            var tags = this.Component.GetSearchTagsOf(PluginContext.Host.SelectedPatient);
            this.SearchTags.Refill(tags);
        }

        private void Remove()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var task = Task.Factory
                .StartNew(context => this.Component.RemoveTasksFor(context as LightPatientDto, this.TagsToRemove), PluginContext.Host.SelectedPatient);
            task.ContinueWith(t =>
            {
                this.IsModified = true;
                this.Close();
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        #endregion Methods
    }
}