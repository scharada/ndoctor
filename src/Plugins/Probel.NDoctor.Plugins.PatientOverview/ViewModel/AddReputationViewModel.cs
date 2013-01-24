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
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddReputationViewModel : InsertionViewModel
    {
        #region Fields

        private ReputationDto reputation;

        #endregion Fields

        #region Constructors

        public AddReputationViewModel()
        {
            this.Reputation = new ReputationDto();
        }

        #endregion Constructors

        #region Properties

        public ReputationDto Reputation
        {
            get { return this.reputation; }
            set
            {
                this.reputation = value;
                this.OnPropertyChanged(() => Reputation);
            }
        }

        #endregion Properties

        #region Methods

        protected override bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(this.Reputation.Name);
        }

        protected override void Insert()
        {
            this.component.Create(this.Reputation);
        }

        #endregion Methods
    }
}