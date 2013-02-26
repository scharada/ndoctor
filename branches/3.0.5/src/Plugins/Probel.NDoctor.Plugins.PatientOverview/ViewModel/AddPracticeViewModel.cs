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

namespace Probel.NDoctor.Plugins.PatientOverview.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class AddPracticeViewModel : InsertionViewModel
    {
        #region Fields

        private PracticeDto practice;

        #endregion Fields

        #region Constructors

        public AddPracticeViewModel()
            : base()
        {
            this.Practice = new PracticeDto();
        }

        #endregion Constructors

        #region Properties

        public PracticeDto Practice
        {
            get { return this.practice; }
            set
            {
                this.practice = value;
                this.OnPropertyChanged(() => Practice);
            }
        }

        #endregion Properties

        #region Methods

        protected override bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Practice.Name);
        }

        protected override void Insert()
        {
            this.component.Create(this.Practice);
        }

        #endregion Methods
    }
}