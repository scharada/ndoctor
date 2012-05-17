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
namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IAdministrationComponent component;
        private Tuple<string, TagCategory> selectedCategory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            if (!Designer.IsDesignMode) this.component = new ComponentFactory(PluginContext.Host.ConnectedUser, PluginContext.ComponentLogginEnabled).GetInstance<IAdministrationComponent>();

            this.Insurances = new ObservableCollection<InsuranceViewModel>();
            this.Practices = new ObservableCollection<PracticeViewModel>();
            this.Pathologies = new ObservableCollection<PathologyViewModel>();
            this.Drugs = new ObservableCollection<DrugViewModel>();
            this.Reputations = new ObservableCollection<ReputationViewModel>();
            this.Professions = new ObservableCollection<ProfessionViewModel>();
            this.Tags = new ObservableCollection<TagViewModel>();

            this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<DrugViewModel> Drugs
        {
            get;
            private set;
        }

        public ObservableCollection<InsuranceViewModel> Insurances
        {
            get;
            private set;
        }

        public ObservableCollection<PathologyViewModel> Pathologies
        {
            get;
            private set;
        }

        public ObservableCollection<PracticeViewModel> Practices
        {
            get;
            private set;
        }

        public ObservableCollection<ProfessionViewModel> Professions
        {
            get;
            private set;
        }

        public ObservableCollection<ReputationViewModel> Reputations
        {
            get;
            private set;
        }

        public Tuple<string, TagCategory> SelectedCategory
        {
            get { return this.selectedCategory; }
            set
            {
                this.selectedCategory = value;
                this.OnPropertyChanged(() => SelectedCategory);
            }
        }

        public ObservableCollection<TagViewModel> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void Refresh()
        {
            IList<InsuranceDto> insurances = null;
            IList<PracticeDto> practices = null;
            IList<PathologyDto> pathologies = null;
            IList<TagDto> tags = null;
            IList<DrugDto> drugs = null;
            IList<ReputationDto> reputations = null;
            IList<ProfessionDto> professions = null;

            using (this.component.UnitOfWork)
            {
                insurances = this.component.GetAllInsurances();
                practices = this.component.GetAllPractices();
                pathologies = this.component.GetAllPathologies();
                tags = this.component.GetAllTags();
                drugs = this.component.GetAllDrugs();
                reputations = this.component.GetAllReputations();
                professions = this.component.GetAllProfessions();
            }

            this.Insurances.Refill(Mapper.Map<IList<InsuranceDto>, IList<InsuranceViewModel>>(insurances));
            this.Practices.Refill(Mapper.Map<IList<PracticeDto>, IList<PracticeViewModel>>(practices));
            this.Pathologies.Refill(Mapper.Map<IList<PathologyDto>, IList<PathologyViewModel>>(pathologies));
            this.Drugs.Refill(Mapper.Map<IList<DrugDto>, IList<DrugViewModel>>(drugs));
            this.Reputations.Refill(Mapper.Map<IList<ReputationDto>, IList<ReputationViewModel>>(reputations));
            this.Tags.Refill(Mapper.Map<IList<TagDto>, IList<TagViewModel>>(tags));
            this.Professions.Refill(Mapper.Map<IList<ProfessionDto>, IList<ProfessionViewModel>>(professions));
        }

        #endregion Methods
    }
}