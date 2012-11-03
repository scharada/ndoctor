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

namespace Probel.NDoctor.Plugins.FamilyViewer.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyViewer.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class AddRelationViewModel : BaseViewModel
    {
        #region Fields

        public static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private readonly ICommand addRelationCommand;
        private readonly IFamilyComponent Component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();
        private readonly ICommand searchCommand;

        private string criteria;
        private bool isBusy;
        private string relation;
        private LightPatientDto selectedMember;
        private Tuple<FamilyRelations, string> selectedRelation;

        #endregion Fields

        #region Constructors

        public AddRelationViewModel()
        {
            this.Relations = new List<Tuple<FamilyRelations, string>>();
            this.Relations.Add(new Tuple<FamilyRelations, string>(FamilyRelations.Parent, this.SetRelation(FamilyRelations.Parent)));
            this.Relations.Add(new Tuple<FamilyRelations, string>(FamilyRelations.Child, this.SetRelation(FamilyRelations.Child)));

            this.FoundMembers = new ObservableCollection<LightPatientDto>();

            this.addRelationCommand = new RelayCommand(() => this.AddRelation(), () => this.CanAddRelation());
            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public ICommand AddRelationCommand
        {
            get { return this.addRelationCommand; }
        }

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                Countdown.Start();
                this.criteria = value;
                this.OnPropertyChanged(() => Criteria);
            }
        }

        public ObservableCollection<LightPatientDto> FoundMembers
        {
            get;
            private set;
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

        public string Relation
        {
            get { return this.relation; }
            set
            {
                this.relation = value;
                this.OnPropertyChanged(() => Relation);
            }
        }

        public List<Tuple<FamilyRelations, string>> Relations
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public LightPatientDto SelectedMember
        {
            get { return this.selectedMember; }
            set
            {
                this.selectedMember = value;
                this.OnPropertyChanged(() => SelectedMember);
            }
        }

        public Tuple<FamilyRelations, string> SelectedRelation
        {
            get { return this.selectedRelation; }
            set
            {
                this.selectedRelation = value;
                this.OnPropertyChanged(() => SelectedRelation);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// With this method, I manage exception of the component and let <see cref="AssertionException"/>
        /// be managed as fatal error
        /// </summary>
        private void AddChild(LightPatientDto selectedPatient, LightPatientDto selectedMember)
        {
            try
            {
                this.Component.AddNewChild(selectedPatient, selectedMember);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        /// <summary>
        /// With this method, I manage exception of the component and let <see cref="AssertionException"/>
        /// be managed as fatal error
        /// </summary>
        private void AddParent(LightPatientDto selectedPatient, LightPatientDto selectedMember)
        {
            try
            {
                this.Component.AddNewParent(selectedPatient, selectedMember);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void AddRelation()
        {
            this.IsBusy = true;
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var task = Task.Factory.StartNew
                (e => this.AddRelationAsync(e as Tuple<LightPatientDto, LightPatientDto, FamilyRelations>)
                , new Tuple<LightPatientDto, LightPatientDto, FamilyRelations>(PluginContext.Host.SelectedPatient, this.SelectedMember, this.SelectedRelation.Item1));

            task.ContinueWith(e => this.AddRelationCallback(), context);
        }

        private void AddRelationAsync(Tuple<LightPatientDto, LightPatientDto, FamilyRelations> e)
        {
            var selectedPatient = e.Item1;
            var selectedMember = e.Item2;
            var selectedRelation = e.Item3;

            switch (selectedRelation)
            {
                case FamilyRelations.Parent:
                    this.AddParent(selectedPatient, selectedMember); // Manage error and let AssertionException be handled as fatal error
                    break;
                case FamilyRelations.Child:
                    this.AddChild(selectedPatient, selectedMember);// Manage error and let AssertionException be handled as fatal error
                    break;
                default:
                    Assert.FailOnEnumeration(this.SelectedRelation.Item1);
                    break;
            }
        }

        private void AddRelationCallback()
        {
            this.IsBusy = false;
            this.OnCloseRequested();
        }

        private bool CanAddRelation()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.SelectedRelation != null
                && this.SelectedMember != null;
        }

        private bool CanSearch()
        {
            return true;
        }

        private void Search()
        {
            try
            {
                var found = this.Component.GetPatientNotFamilyMembers(PluginContext.Host.SelectedPatient, this.Criteria, SearchOn.FirstAndLastName);
                this.FoundMembers.Refill(found);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private string SetRelation(FamilyRelations relation)
        {
            return string.Format(Messages.Msg_Relation, relation.Translate(), PluginContext.Host.SelectedPatient.DisplayedName);
        }

        #endregion Methods
    }
}