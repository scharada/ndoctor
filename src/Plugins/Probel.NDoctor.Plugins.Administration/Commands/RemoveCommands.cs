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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class RemoveCommands : BaseCommands
    {
        #region Constructors

        public RemoveCommands(WorkbenchViewModel viewModel, IErrorHandler errorHandler)
            : base(viewModel, errorHandler)
        {
            this.InsuranceCommand = new RelayCommand(() => this.RemoveInsurance(), () => this.ViewModel.SelectedInsurance != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.PracticeCommand = new RelayCommand(() => this.RemovePractice(), () => this.ViewModel.SelectedPractice != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.PathologyCommand = new RelayCommand(() => this.RemovePathology(), () => this.ViewModel.SelectedPathology != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.DrugCommand = new RelayCommand(() => this.RemoveDrug(), () => this.ViewModel.SelectedDrug != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.ProfessionCommand = new RelayCommand(() => this.RemoveProfession(), () => this.ViewModel.SelectedProfession != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.ReputationCommand = new RelayCommand(() => this.RemoveReputation(), () => this.ViewModel.SelectedReputation != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.TagCommand = new RelayCommand(() => this.RemoveTag(), () => this.ViewModel.SelectedTag != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.DoctorCommand = new RelayCommand(() => this.RemoveDoctor(), () => this.ViewModel.SelectedDoctor != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
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

        private void RemoveDoctor()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedDoctor))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedDoctor);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveDrug()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedDrug))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedDrug);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveInsurance()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedInsurance))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        Component.Remove(this.ViewModel.SelectedInsurance);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePathology()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedPathology))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedPathology);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePractice()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedPractice))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedPractice);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveProfession()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedProfession))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedProfession);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveReputation()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedReputation))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedReputation);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }

            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveTag()
        {
            try
            {
                if (this.Component.CanRemove(this.ViewModel.SelectedTag))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.Component.Remove(this.ViewModel.SelectedTag);
                        this.ViewModel.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool UserAcceptedDeletion()
        {
            return ViewService.MessageBox.Question(Messages.Msg_AskDelete);
        }

        #endregion Methods
    }
}