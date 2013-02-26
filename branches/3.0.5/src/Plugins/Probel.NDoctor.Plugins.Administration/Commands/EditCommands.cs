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

namespace Probel.NDoctor.Plugins.Administration.Commands
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class EditCommands : BaseCommands
    {
        #region Constructors

        public EditCommands(WorkbenchViewModel viewModel, IErrorHandler errorHandler)
            : base(viewModel, errorHandler)
        {
            this.InsuranceCommand = new RelayCommand(() => this.EditInsurance(), () => this.ViewModel.SelectedInsurance != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.ProfessionCommand = new RelayCommand(() => EditProfession(), () => this.ViewModel.SelectedProfession != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.PracticeCommand = new RelayCommand(() => this.EditPractice(), () => this.ViewModel.SelectedPractice != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.PathologyCommand = new RelayCommand(() => this.EditPathology(), () => this.ViewModel.SelectedPathology != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.DrugCommand = new RelayCommand(() => this.EditDrug(), () => this.ViewModel.SelectedDrug != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.ReputationCommand = new RelayCommand(() => this.EditReputation(), () => this.ViewModel.SelectedReputation != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.TagCommand = new RelayCommand(() => this.EditTag(), () => this.ViewModel.SelectedTag != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.DoctorCommand = new RelayCommand(() => this.EditDoctor(), () => this.ViewModel.SelectedDoctor != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
        }

        #endregion Constructors

        #region Properties

        public ICommand DoctorCommand
        {
            get;
            private set;
        }

        public ICommand DrugCommand
        {
            get;
            private set;
        }

        public ICommand InsuranceCommand
        {
            get;
            private set;
        }

        public ICommand PathologyCommand
        {
            get;
            private set;
        }

        public ICommand PracticeCommand
        {
            get;
            private set;
        }

        public ICommand ProfessionCommand
        {
            get;
            private set;
        }

        public ICommand ReputationCommand
        {
            get;
            private set;
        }

        public ICommand TagCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void EditDoctor()
        {
            ViewService.Manager.ShowDialog<AddDoctorViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedDoctor);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedDoctor;
            });
        }

        private void EditDrug()
        {
            ViewService.Manager.ShowDialog<AddDrugViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedDrug);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedDrug;
            });
        }

        private void EditInsurance()
        {
            ViewService.Manager.ShowDialog<AddInsuranceViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedInsurance);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedInsurance;
            });
        }

        private void EditPathology()
        {
            ViewService.Manager.ShowDialog<AddPathologyViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedPathology);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedPathology;
            });
        }

        private void EditPractice()
        {
            ViewService.Manager.ShowDialog<AddPracticeViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedPractice);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedPractice;
            });
        }

        private void EditProfession()
        {
            ViewService.Manager.ShowDialog<AddProfessionViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedProfession);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedProfession;
            });
        }

        private void EditReputation()
        {
            ViewService.Manager.ShowDialog<AddReputationViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.Component.Update(this.ViewModel.SelectedReputation);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.ViewModel.SelectedReputation;
            });
        }

        private void EditTag()
        {
            ViewService.Manager.ShowDialog<EditTagViewModel>(vm => vm.BoxItem = this.ViewModel.SelectedTag);
        }

        #endregion Methods
    }
}