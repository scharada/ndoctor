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
namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;

    public class DrugViewModel : DrugDto
    {
        #region Fields

        private bool isSelected;

        #endregion Fields

        #region Properties

        public string BtnSelect
        {
            get { return Messages.Btn_Select; }
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

        public AddPrescriptionViewModel Parent
        {
            get; set;
        }

        public ICommand SelectDrugCommand
        {
            get
            {
                Assert.IsNotNull(this.Parent, "The parent of {0} should be set.", this.GetType());
                return this.Parent.SelectDrugCommand;
            }
        }

        #endregion Properties
    }
}