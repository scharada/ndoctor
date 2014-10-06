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
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Properties;
    using Probel.NDoctor.View.Plugins;

    internal class AddTagViewModel : TagViewModel
    {
        #region Fields

        private readonly ICommand addCommand;
        private readonly ICommand closeViewCommand;

        private SearchTagDto newSearchTag;

        #endregion Fields

        #region Constructors

        public AddTagViewModel()
        {
            this.newSearchTag = new SearchTagDto();
            this.addCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.closeViewCommand = new RelayCommand(() => this.Close());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get { return this.addCommand; }
        }

        public ICommand CloseViewCommand
        {
            get { return this.closeViewCommand; }
        }

        public SearchTagDto NewSearchTag
        {
            get { return this.newSearchTag; }
            set
            {
                this.newSearchTag = value;
                this.OnPropertyChanged(() => NewSearchTag);
            }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            this.IsModified = true;
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            if (!this.Component.CheckSearchTagExist(this.NewSearchTag.Name))
            {
                var task = Task.Factory
                     .StartNew(context => this.Component.AddTagTo(context as LightPatientDto, this.NewSearchTag), PluginContext.Host.SelectedPatient);
                task.ContinueWith(t => this.Close(), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
            else { ViewService.MessageBox.Warning(Messages.Msg_WarnSearchTagExists); }
        }

        private bool CanAdd()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        #endregion Methods
    }
}