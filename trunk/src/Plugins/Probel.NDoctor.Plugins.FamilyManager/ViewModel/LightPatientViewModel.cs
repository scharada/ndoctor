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
namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Events;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class LightPatientViewModel : LightPatientDto
    {
        #region Fields

        private IFamilyComponent component = new ComponentFactory().GetInstance<IFamilyComponent>();
        private bool isSelected = false;
        private Tuple<FamilyRelations, string> selectedRelation;
        private LightPatientDto sessionPatient;

        #endregion Fields

        #region Constructors

        public LightPatientViewModel()
        {
            this.Relations = new List<Tuple<FamilyRelations, string>>();

            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.RemoveCommand = new RelayCommand(() => this.Delete());
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
                this.OnPropertyChanged(() => IsSelected);
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
                this.OnPropertyChanged(() => SelectedRelation);
            }
        }

        public LightPatientDto SessionPatient
        {
            get { return this.sessionPatient; }
            set
            {
                this.sessionPatient = value;
                this.OnPropertyChanged(() => SessionPatient);

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

            //this.State = State.Created;
            using (this.component.UnitOfWork)
            {
                this.component.Update(this.BuildFamily());
            }
            this.OnRefreshed(State.Created);
            this.BuildFamily();

            Notifyer.OnRefreshed(this);
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

        private void Delete()
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

        private void OnRefreshed(State state)
        {
            if (this.Refreshed != null)
                this.Refreshed(this, new EventArgs<State>(state));
        }

        private void SetParent(FamilyDto family, LightPatientDto current)
        {
            switch (current.Gender)
            {
                case Gender.Male:
                    family.Fathers.Clear();
                    family.Fathers.Add(current);
                    break;
                case Gender.Female:
                    family.Mothers.Clear();
                    family.Mothers.Add(current);
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