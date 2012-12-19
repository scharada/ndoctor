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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

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
    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox.Logging;

    internal class StartPageViewModel : BaseViewModel
    {

        private LogEvent selectedRow;
        public LogEvent SelectedRow
        {
            get { return this.selectedRow; }
            set
            {
                this.selectedRow = value;
                this.OnPropertyChanged(() => SelectedRow);
            }
        }     
        #region Fields

        private readonly IApplicationStatisticsComponent Component = PluginContext.ComponentFactory.GetInstance<IApplicationStatisticsComponent>();
        private readonly ICommand refreshStatisticsCommand;

        private Chart<DateTime, double> executionTime;
        private bool isBusy;
        private Chart<string, double> targetUsage;

        #endregion Fields

        #region Constructors

        public StartPageViewModel()
        {
            this.refreshStatisticsCommand = new RelayCommand(() => this.RefreshStatistics(), () => this.CanRefreshStatistics());
            this.Bottlenecks = new ObservableCollection<BottleneckDto>();
            this.LogEvents = new ObservableCollection<LogEvent>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<BottleneckDto> Bottlenecks
        {
            get;
            private set;
        }

        public Chart<DateTime, double> ExecutionTime
        {
            get { return this.executionTime; }
            set
            {
                this.executionTime = value;
                this.OnPropertyChanged(() => ExecutionTime);
            }
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(() => IsBusy);
            }
        }

        /// <summary>
        /// Gets the list of log events recorded in this session.
        /// </summary>
        public ObservableCollection<LogEvent> LogEvents
        {
            get;
            set;
        }

        public ICommand RefreshStatisticsCommand
        {
            get { return this.refreshStatisticsCommand; }
        }

        public Chart<string, double> TargetUsage
        {
            get { return this.targetUsage; }
            set
            {
                this.targetUsage = value;
                this.OnPropertyChanged(() => TargetUsage);
            }
        }

        #endregion Properties

        #region Methods

        private bool CanRefreshStatistics()
        {
            return true;
        }

        private void RefreshStatistics()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;
            var taskContext = new TaskContext();

            var task = Task.Factory.StartNew<TaskContext>(e => this.RefreshStatisticsAsync(), taskContext, token, TaskCreationOptions.None, context);

            this.IsBusy = true;
            task.ContinueWith(e => RefreshStatisticsCallback(e.Result)
                , token, TaskContinuationOptions.OnlyOnRanToCompletion, context);
            task.ContinueWith(e => this.Handle.Error(e.Exception.InnerException.InnerException)
                , token, TaskContinuationOptions.OnlyOnFaulted, context);
        }

        private TaskContext RefreshStatisticsAsync()
        {
            var context = new TaskContext();

            context.TargetUsage = this.Component.GetUsageByTargets();
            context.ExecutionTime = this.Component.ExecutionTimeGraph();
            context.Bottlenecks = this.Component.GetBottlenecksArray();
            return context;
        }

        private void RefreshStatisticsCallback(TaskContext taskContext)
        {
            this.TargetUsage = taskContext.TargetUsage;
            this.ExecutionTime = taskContext.ExecutionTime;
            this.Bottlenecks.Refill(taskContext.Bottlenecks);
            this.LogEvents.Refill(WpfAppender.GetLogs(this.Logger));
            this.IsBusy = false;
        }

        #endregion Methods

        #region Nested Types

        private class TaskContext
        {
            #region Properties

            public IEnumerable<BottleneckDto> Bottlenecks
            {
                get;
                set;
            }

            public Chart<DateTime, double> ExecutionTime
            {
                get;
                set;
            }

            public Chart<string, double> TargetUsage
            {
                get;
                set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}