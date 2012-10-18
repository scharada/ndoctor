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
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class LightPatientViewModel : LightPatientDto
    {
        #region Fields

        private readonly IErrorHandler Handle;

        private IFamilyComponent component;
        private bool isSelected = false;
        private Tuple<FamilyRelations, string> selectedRelation;
        private LightPatientDto sessionPatient;

        #endregion Fields

        #region Constructors

        public LightPatientViewModel()
        {
            this.Handle = new ErrorHandlerFactory().New(this);
            this.component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();

            this.Relations = new List<Tuple<FamilyRelations, string>>();

            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.RemoveCommand = new RelayCommand(() => this.Delete());
        }

        #endregion Constructors

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

            try
            {
                this.component.Update(this.BuildFamily());
                Notifyer.OnRefreshed(this);
                this.BuildFamily();

                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
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

            try
            {
                var member = Mapper.Map<LightPatientViewModel, LightPatientDto>(this);

                this.component.RemoveFamilyMember(member, this.SessionPatient);

                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
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