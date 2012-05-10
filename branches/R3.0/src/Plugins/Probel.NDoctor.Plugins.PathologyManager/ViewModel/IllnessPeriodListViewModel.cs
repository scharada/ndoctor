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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Helpers;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class IllnessPeriodListViewModel : BaseViewModel
    {
        #region Fields

        private IFamilyComponent component = ObjectFactory.GetInstance<IFamilyComponent>();
        private string criteria;
        private IllnessPeriodToAddViewModel selectedPathology;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IllnessPeriodListViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public IllnessPeriodListViewModel()
            : base()
        {
            this.FoundPathologies = new ObservableCollection<IllnessPeriodToAddViewModel>();
            this.SearchCommand = new RelayCommand(() => this.SearchPathology(), () => this.CanSearchPathology());

            Notifyer.PathologyAdded += (sender, e) =>
            {
                this.Criteria = e.Data;
                this.SearchCommand.Execute(null);
            };
        }

        #endregion Constructors

        #region Properties

        public string BtnSearch
        {
            get { return Messages.Btn_Search; }
        }

        /// <summary>
        /// Gets or sets the criteria for the pathology search.
        /// </summary>
        /// <value>
        /// The criteria.
        /// </value>
        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                this.criteria = value;
                this.OnPropertyChanged("Criteria");
            }
        }

        /// <summary>
        /// Gets the found pathologies.
        /// </summary>
        public ObservableCollection<IllnessPeriodToAddViewModel> FoundPathologies
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public IllnessPeriodToAddViewModel SelectedPathology
        {
            get { return this.selectedPathology; }
            set
            {
                this.selectedPathology = value;
                this.OnPropertyChanged("SelectedPathology");
            }
        }

        #endregion Properties

        #region Methods

        private bool CanSearchPathology()
        {
            return !string.IsNullOrWhiteSpace(this.Criteria);
        }

        private void SearchPathology()
        {
            IList<PathologyDto> pathologies;
            using (this.component.UnitOfWork)
            {
                pathologies = this.component.FindPathology(this.criteria);
            }

            var result = Mapper.Map<IList<PathologyDto>, IList<IllnessPeriodToAddViewModel>>(pathologies);
            this.FoundPathologies.Refill(result);
        }

        #endregion Methods
    }
}