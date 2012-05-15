namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class LightPatientViewModel : LightPatientDto
    {
        #region Fields

        private IFamilyComponent component = ComponentFactory.FamilyComponent;
        private bool isSelected = false;
        private Tuple<FamilyRelations, string> selectedRelation;
        private LightPatientDto sessionPatient;

        #endregion Fields

        #region Constructors

        public LightPatientViewModel()
        {
            this.Relations = new List<Tuple<FamilyRelations, string>>();

            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.RemoveCommand = new RelayCommand(() => this.Remove());
        }

        #endregion Constructors

        #region Events

        public event EventHandler<EventArgs<State>> Refreshed;

        #endregion Events

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
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
                this.OnPropertyChanged("IsSelected");
            }
        }

        public List<Tuple<FamilyRelations, string>> Relations
        {
            get;
            private set;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public Tuple<FamilyRelations, string> SelectedRelation
        {
            get { return this.selectedRelation; }
            set
            {
                this.selectedRelation = value;
                this.OnPropertyChanged("SelectedRelation");
            }
        }

        public LightPatientDto SessionPatient
        {
            get { return this.sessionPatient; }
            set
            {
                this.sessionPatient = value;
                this.OnPropertyChanged("SessionPatient");

                this.Relations = new List<Tuple<FamilyRelations, string>>();
                this.Relations.Add(new Tuple<FamilyRelations, string>(FamilyRelations.Parent, this.SetRelation(FamilyRelations.Parent)));
                this.Relations.Add(new Tuple<FamilyRelations, string>(FamilyRelations.Child, this.SetRelation(FamilyRelations.Child)));
            }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            var dr = MessageBox.Show(Messages.Msg_AskAddMember
                , Messages.Question
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question);
            if (dr == MessageBoxResult.No) return;

            this.State = State.Added;
            using (this.component.UnitOfWork)
            {
                this.component.Update(this.BuildFamily());
            }
            this.OnRefreshed(State.Added);
        }

        private FamilyDto BuildFamily()
        {
            var family = new FamilyDto() { Current = this.SessionPatient };
            var current = Mapper.Map<LightPatientViewModel, LightPatientDto>(this);

            switch (this.SelectedRelation.Item1)
            {
                case FamilyRelations.Parent:
                    this.SetParent(family, current);
                    break;
                case FamilyRelations.Child:
                    family.Children.Add(current);
                    break;
                default:
                    Assert.FailOnEnumeration(this.SelectedRelation.Item1);
                    break;
            }
            return family;
        }

        private bool CanAdd()
        {
            return this.SelectedRelation != null;
        }

        private void OnRefreshed(State state)
        {
            if (this.Refreshed != null)
                this.Refreshed(this, new EventArgs<State>(state));
        }

        private void Remove()
        {
            var dr = MessageBox.Show(Messages.Msg_AskRemoveMember
                , Messages.Question
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question);
            if (dr == MessageBoxResult.No) return;

            var member = Mapper.Map<LightPatientViewModel, LightPatientDto>(this);
            using (this.component.UnitOfWork)
            {
                this.component.RemoveFamilyMember(member, this.SessionPatient);
            }
            this.OnRefreshed(State.Removed);
        }

        private void SetParent(FamilyDto family, LightPatientDto current)
        {
            switch (current.Gender)
            {
                case Gender.Male:
                    family.Father = current;
                    break;
                case Gender.Female:
                    family.Mother = current;
                    break;
                default:
                    Assert.FailOnEnumeration(family.Current.Gender);
                    break;
            }
        }

        private string SetRelation(FamilyRelations relation)
        {
            Assert.IsNotNull(this.SessionPatient, "Member");
            return string.Format(Messages.Msg_Relation, relation.Translate(), this.SessionPatient.DisplayedName);
        }

        #endregion Methods
    }
}