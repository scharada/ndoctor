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

namespace Probel.NDoctor.Plugins.PatientSession.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Specification;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SearchPatientExtendedViewModel : BaseViewModel
    {
        #region Fields
        private readonly IPatientSessionComponent Component;
        private SpecificationExpression<PatientDto> expression;
        private string selectedSpecification;
        private string value;

        #endregion Fields

        #region Constructors
        private void Search()
        {
            var patients = this.Component.FindPatientsByNameLight("*", this.expression);
            this.FoundPatients.Refill(patients);
        }
        public SearchPatientExtendedViewModel()
        {
            this.Component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();

            this.FoundPatients = new ObservableCollection<LightPatientDto>();
            this.Specifications = new ObservableCollection<string>();
            this.Specifications.Add(Messages.Criteria_ByProfession);
            this.Specifications.Add(Messages.Criteria_ByYear);
            this.Specifications.Add(Messages.Criteria_ByName);

            this.AddSpecificationCommand = new RelayCommand(() => this.AddSpecification(), () => this.CanAddSpecification());
            this.SearchCommand = new RelayCommand(() => this.Search());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddSpecificationCommand
        {
            get;
            private set;
        }

        public ICommand SearchCommand { get; set; }

        public ObservableCollection<LightPatientDto> FoundPatients
        {
            get;
            set;
        }

        public string SelectedSpecification
        {
            get { return this.selectedSpecification; }
            set
            {
                this.selectedSpecification = value;
                this.OnPropertyChanged(() => SelectedSpecification);
            }
        }

        public ObservableCollection<string> Specifications
        {
            get;
            private set;
        }

        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.OnPropertyChanged(() => Value);
            }
        }

        #endregion Properties

        #region Methods

        private void AddSpecification()
        {
            try
            {
                if (this.expression == null)
                {
                    this.expression = this.BuildExpression();
                }
                else if (this.SelectedSpecification == Messages.Criteria_ByProfession)
                {
                    this.expression.And(new FindPatientByProfessionSpecification(this.Value));
                }
                else if (this.SelectedSpecification == Messages.Criteria_ByYear)
                {
                    int year;
                    if (Int32.TryParse(this.Value, out year))
                    {
                        this.expression.And(new FindPatientByBirthYearSpecification(year));
                    }
                    else { throw new ArgumentException(Messages.Ex_NotNumericValue); }
                }
                else if (this.SelectedSpecification == Messages.Criteria_ByName) { this.expression.And(new FindPatientByNameSpecification(this.Value)); }
                else { throw new NotSupportedException("The specification '{0}'is not yet supported".FormatWith(this.Value)); }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private SpecificationExpression<PatientDto> BuildExpression()
        {
            if (this.SelectedSpecification == Messages.Criteria_ByProfession)
            {
                return new SpecificationExpression<PatientDto>(new FindPatientByProfessionSpecification(this.Value));
            }
            else if (this.SelectedSpecification == Messages.Criteria_ByYear)
            {
                int year;
                if (Int32.TryParse(this.Value, out year))
                {
                    return new SpecificationExpression<PatientDto>(new FindPatientByBirthYearSpecification(year));
                }
                else { throw new ArgumentException(Messages.Ex_NotNumericValue); }
            }
            else if (this.SelectedSpecification == Messages.Criteria_ByName)
            {
                return new SpecificationExpression<PatientDto>(new FindPatientByNameSpecification(this.Value));
            }
            else { throw new NotSupportedException("The specification '{0}'is not yet supported".FormatWith(this.Value)); }
        }

        private bool CanAddSpecification()
        {
            return !string.IsNullOrEmpty(this.SelectedSpecification)
                && !string.IsNullOrEmpty(this.Value);
        }

        #endregion Methods
    }
}