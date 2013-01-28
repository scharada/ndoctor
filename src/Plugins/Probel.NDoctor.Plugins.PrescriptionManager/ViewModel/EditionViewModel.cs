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

namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Windows.Input;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Toolbox.Helpers;
    using Probel.NDoctor.View.Plugins;

    internal class EditionViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPrescriptionComponent Component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
        private readonly ICommand editCommand;

        private PrescriptionDto selectedPrescription;
        private ReferencedObject<string> selectedText;

        #endregion Fields

        #region Constructors

        public EditionViewModel()
        {
            this.editCommand = new RelayCommand(() => this.Edit(), () => this.CanEdit());
        }

        #endregion Constructors

        #region Properties

        public ICommand EditCommand
        {
            get { return this.editCommand; }
        }

        public PrescriptionDto SelectedPrescription
        {
            get { return this.selectedPrescription; }
            set
            {
                this.selectedPrescription = value;
                this.SelectedText = new ReferencedObject<string>(value.Notes);
                this.OnPropertyChanged(() => SelectedPrescription);
            }
        }

        public ReferencedObject<string> SelectedText
        {
            get { return this.selectedText; }
            set
            {
                this.selectedText = value;
                this.OnPropertyChanged(() => SelectedText);
            }
        }

        #endregion Properties

        #region Methods

        private bool CanEdit()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void Edit()
        {
            try
            {
                if (this.SelectedPrescription != null)
                {
                    this.SelectedPrescription.Notes = this.SelectedText.Value;
                    this.Component.Update(this.SelectedPrescription);
                    this.Close();
                }
                else { this.Logger.Warn("The Selected prescription is null"); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}