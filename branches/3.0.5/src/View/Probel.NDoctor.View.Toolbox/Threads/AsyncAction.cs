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

namespace Probel.NDoctor.View.Toolbox.Threads
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Probel.NDoctor.View.Toolbox.Navigation;

    /// <summary>
    /// Execute an asynchronous business logic action and refreshes the UI when it's done
    /// </summary>
    public class AsyncAction
    {
        #region Fields

        private readonly IErrorHandler Handle;
        private readonly TaskScheduler Scheduler;
        private readonly CancellationToken Token = new CancellationTokenSource().Token;

        #endregion Fields

        #region Constructors

        public AsyncAction(IErrorHandler errorHandler)
            : this(TaskScheduler.FromCurrentSynchronizationContext(), errorHandler)
        {
        }

        public AsyncAction(TaskScheduler scheduler, IErrorHandler errorHandler)
        {
            this.Scheduler = scheduler;
            this.Handle = errorHandler;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Executes asynchronously the businessLogic and when it finished, refreshes the UI.
        /// </summary>
        /// <typeparam name="T">The type of the returned data from the business logic thread</typeparam>
        /// <param name="businessLogic">The business logic.</param>
        /// <param name="refreshUI">The refresh UI.</param>
        public void ExecuteAsync<T>(Func<T> businessLogic, Action<T> refreshUI)
        {
            var task = Task.Factory.StartNew<T>(() => businessLogic());
            task.ContinueWith(t => refreshUI(t.Result), this.Token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), this.Token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        /// <summary>
        /// Executes the specified business logic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="businessLogic">The business logic.</param>
        /// <param name="context">The context.</param>
        /// <param name="refreshUI">The refresh UI.</param>
        public void ExecuteAsync<T, TContext>(Func<TContext, T> businessLogic, object context, Action<T> refreshUI)
        {
            var task = Task.Factory.StartNew<T>(ctx => { return businessLogic((TContext)ctx); }, context);
            task.ContinueWith(t => refreshUI(t.Result), this.Token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), this.Token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        /// <summary>
        /// Executes asynchronously the businessLogic and when it finished, refreshes the UI.
        /// </summary>
        /// <param name="businessLogic">The business logic.</param>
        /// <param name="refreshUI">The refresh UI.</param>
        public void ExecuteAsync(Action businessLogic, Action refreshUI)
        {
            var task = Task.Factory.StartNew(() => businessLogic());
            task.ContinueWith(t => refreshUI(), this.Token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), this.Token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        #endregion Methods
    }
}