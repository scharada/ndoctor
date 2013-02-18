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

namespace Probel.NDoctor.Plugins.Administration
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using log4net;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Memory;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class WorkbenchRefresher
    {
        #region Fields

        private readonly IErrorHandler Handle;
        private readonly ILog Logger = LogManager.GetLogger(typeof(WorkbenchRefresher));
        private readonly TaskScheduler Scheduler;
        private readonly WorkbenchViewModel ViewModel;

        private bool swallowEmptyModuleWarning = false;

        #endregion Fields

        #region Constructors

        public WorkbenchRefresher(WorkbenchViewModel viewModel, IAdministrationComponent component, IErrorHandler errorHandler)
        {
            this.ViewModel = viewModel;
            this.Component = component;
            this.Handle = errorHandler;
            this.Scheduler = viewModel.Scheduler;
            Thread.CurrentThread.CurrentUICulture = this.ViewModel.Culture;
        }

        #endregion Constructors

        #region Properties

        public IAdministrationComponent Component
        {
            get;
            set;
        }

        private DoctorRefiner DoctorRefiner
        {
            get;
            set;
        }

        private DrugRefiner DrugRefiner
        {
            get;
            set;
        }

        private InsuranceRefiner InsuranceRefiner
        {
            get;
            set;
        }

        private PathologyRefiner PathologyRefiner
        {
            get;
            set;
        }

        private PracticeRefiner PracticeRefiner
        {
            get;
            set;
        }

        private ProfessionRefiner ProfessionRefiner
        {
            get;
            set;
        }

        private ReputationRefiner ReputationRefiner
        {
            get;
            set;
        }

        private TagRefiner TagRefiner
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the ViewModel with all the administration data.
        /// </summary>
        public void Refresh()
        {
            this.ViewModel.IsInsuranceBusy
                = this.ViewModel.IsTagBusy
                = this.ViewModel.IsReputationBusy
                = this.ViewModel.IsProfessionBusy
                = this.ViewModel.IsDrugBusy
                = this.ViewModel.IsPathologyBusy
                = this.ViewModel.IsPracticeBusy
                = this.ViewModel.IsDoctorBusy
                = true;

            var token = new CancellationTokenSource().Token;
            this.RefreshInsurances(token);
            this.RefreshTags(token);
            this.RefreshReputations(token);
            this.RefreshProfessions(token);
            this.RefreshDrugs(token);
            this.RefreshPathologies(token);
            this.RefreshPractices(token);
            this.RefreshDoctors(token);
        }

        /// <summary>
        /// Make a memory search based on the specified criteria and refresh the displayed doctors.
        /// </summary>
        /// <param name="name">The last name.</param>
        public void RefreshDoctorInMemory(string name)
        {
            if (this.DoctorRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsDoctorBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<DoctorDto>>(() => this.DoctorRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Doctors.Refill(t.Result);
                    this.ViewModel.IsDoctorBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshDrugInMemory(string name)
        {
            if (this.DrugRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsDrugBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<DrugDto>>(() => this.DrugRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Drugs.Refill(t.Result);
                    this.ViewModel.IsDrugBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }            
        }

        public void RefreshInsurancesInMemory(string name)
        {
            if (this.InsuranceRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsInsuranceBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<InsuranceDto>>(() => this.InsuranceRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Insurances.Refill(t.Result);
                    this.ViewModel.IsInsuranceBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshPathologyInMemory(string name)
        {
            if (this.PathologyRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsPathologyBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<PathologyDto>>(() => this.PathologyRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Pathologies.Refill(t.Result);
                    this.ViewModel.IsPathologyBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshPracticeInMemory(string name)
        {
            if (this.PracticeRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsPracticeBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<PracticeDto>>(() => this.PracticeRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Practices.Refill(t.Result);
                    this.ViewModel.IsPracticeBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshProfessionInMemory(string name)
        {
            if (this.ProfessionRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsProfessionBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<ProfessionDto>>(() => this.ProfessionRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Professions.Refill(t.Result);
                    this.ViewModel.IsProfessionBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshReputationInMemory(string name)
        {
            if (this.ReputationRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsReputationBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<ReputationDto>>(() => this.ReputationRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Reputations.Refill(t.Result);
                    this.ViewModel.IsReputationBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        public void RefreshTagInMemory(string name)
        {
            if (this.TagRefiner != null)
            {
                var token = new CancellationTokenSource().Token;
                this.ViewModel.IsTagBusy = true;
                var task = Task.Factory.StartNew<IEnumerable<TagDto>>(() => this.TagRefiner.GetByName(name));
                task.ContinueWith(t =>
                {
                    this.ViewModel.Tags.Refill(Mapper.Map<IEnumerable<TagDto>, IEnumerable<TagViewModel>>(t.Result));
                    this.ViewModel.IsTagBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
            }
        }

        private void RefreshDoctors(CancellationToken token)
        {
            var task = Task.Factory.StartNew<DoctorRefiner>(() => this.Component.GetDoctorRefiner());
            task.ContinueWith(t =>
            {
                this.DoctorRefiner = t.Result;
                this.ViewModel.Doctors.Refill(this.DoctorRefiner.Items);
                this.ViewModel.IsDoctorBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshDrugs(CancellationToken token)
        {
            var task = Task.Factory.StartNew<DrugRefiner>(() => this.Component.GetDrugRefiner());
            task.ContinueWith(t =>
            {
                this.DrugRefiner = t.Result;
                this.ViewModel.Drugs.Refill(t.Result.Items);
                this.ViewModel.IsDrugBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshInsurances(CancellationToken token)
        {
            var task = Task.Factory.StartNew<InsuranceRefiner>(() => this.Component.GetInsurancesRefiner());
            task.ContinueWith(t =>
            {
                this.InsuranceRefiner = t.Result;
                this.ViewModel.Insurances.Refill(t.Result.Items);
                this.ViewModel.IsInsuranceBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshPathologies(CancellationToken token)
        {
            var task = Task.Factory.StartNew<PathologyRefiner>(() => this.Component.GetPathologyRefiner());
            task.ContinueWith(t =>
            {
                this.PathologyRefiner = t.Result;
                this.ViewModel.Pathologies.Refill(t.Result.Items);
                this.ViewModel.IsPathologyBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshPractices(CancellationToken token)
        {
            var task = Task.Factory.StartNew<PracticeRefiner>(() => this.Component.GetPracticeRefiner());
            task.ContinueWith(t =>
            {
                this.PracticeRefiner = t.Result;
                this.ViewModel.Practices.Refill(t.Result.Items);
                this.ViewModel.IsPracticeBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshProfessions(CancellationToken token)
        {
            var task = Task.Factory.StartNew<ProfessionRefiner>(() => this.Component.GetProfessionRefiner());
            task.ContinueWith(t =>
            {
                this.ProfessionRefiner = t.Result;
                this.ViewModel.Professions.Refill(t.Result.Items);
                this.ViewModel.IsProfessionBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshReputations(CancellationToken token)
        {
            var task = Task.Factory.StartNew<ReputationRefiner>(() => this.Component.GetReputationRefiner());
            task.ContinueWith(t =>
            {
                this.ReputationRefiner = t.Result;
                this.ViewModel.Reputations.Refill(t.Result.Items);
                this.ViewModel.IsReputationBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        private void RefreshTags(CancellationToken token)
        {
            var task = Task.Factory.StartNew<TagRefiner>(() => this.Component.GetTagRefiner());
            task.ContinueWith(t =>
            {
                this.TagRefiner = t.Result;
                var result = Mapper.Map<IEnumerable<TagDto>, IEnumerable<TagViewModel>>(t.Result.Items);
                this.ViewModel.Tags.Refill(result);
                this.ViewModel.IsTagBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, this.Scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, this.Scheduler);
        }

        #endregion Methods
    }
}