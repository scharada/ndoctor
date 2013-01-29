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

namespace Probel.NDoctor.Plugins.PictureManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui.FileServices;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PictureManager.Properties;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    public class AddPictureViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPictureComponent Component;
        private readonly PictureDto PicToAdd = new PictureDto();
        private readonly ICommand refreshCommand;
        private readonly ICommand savePictureCommand;
        private readonly ICommand selectFileCommand;

        private bool hasAddPicture = false;
        private bool isBusy;
        private string notes;
        private TagDto selectedCategory;
        private string selectedFile;

        #endregion Fields

        #region Constructors

        public AddPictureViewModel()
        {
            this.Component = PluginContext.ComponentFactory.GetInstance<IPictureComponent>();
            this.Categories = new ObservableCollection<TagDto>();

            this.refreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.selectFileCommand = new RelayCommand(() => this.SelectFile(), () => this.CanSelectFile());
            this.savePictureCommand = new RelayCommand(() => this.SavePicture(), () => this.CanSavePicture());
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> Categories
        {
            get;
            private set;
        }

        public bool HasAddPicture
        {
            get { return this.hasAddPicture; }
            set
            {
                this.hasAddPicture = value;
                this.OnPropertyChanged(() => HasAddPicture);
            }
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(() => IsBusy);
            }
        }

        public string Notes
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged(() => Notes);
            }
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand SavePictureCommand
        {
            get { return this.savePictureCommand; }
        }

        public TagDto SelectedCategory
        {
            get { return this.selectedCategory; }
            set
            {
                this.selectedCategory = value;
                this.OnPropertyChanged(() => SelectedCategory);
            }
        }

        public string SelectedFile
        {
            get { return this.selectedFile; }
            set
            {
                this.selectedFile = value;
                this.OnPropertyChanged(() => SelectedFile);
            }
        }

        public ICommand SelectFileCommand
        {
            get { return this.selectFileCommand; }
        }

        #endregion Properties

        #region Methods

        private bool CanRefresh()
        {
            return true;
        }

        private bool CanSavePicture()
        {
            return File.Exists(this.SelectedFile)
                && this.SelectedCategory != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSelectFile()
        {
            return true;
        }

        private void Refresh()
        {
            try
            {
                this.Categories.Refill(this.Component.GetTags(TagCategory.Picture));
                if (Categories.Count > 0) { this.SelectedCategory = this.Categories[0]; }
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex);
            }
        }

        private void SavePicture()
        {
            try
            {
                this.IsBusy = true;

                var context = TaskScheduler.FromCurrentSynchronizationContext();
                var args = new Tuple<PictureDto, LightPatientDto>(this.PicToAdd, PluginContext.Host.SelectedPatient);
                Task.Factory.StartNew(e => this.SavePictureAsync(e as Tuple<PictureDto, LightPatientDto>), args)
                            .ContinueWith(e => this.SavePictureCallback(), context);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SavePictureAsync(Tuple<PictureDto, LightPatientDto> context)
        {
            try
            {
                context.Item1.Tag = this.SelectedCategory;
                context.Item1.Notes = this.Notes;

                this.Component.Create(context.Item1, context.Item2);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SavePictureCallback()
        {
            try
            {
                this.HasAddPicture = true;
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PictureAdded);
                this.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SelectFile()
        {
            var opt = new Options()
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF",
                Multiselect = false,
                Title = Messages.Title_SelectPicture,
            };
            var dr = FileGuiFactory.Win32.SelectFile(e => this.SelectedFile = e, opt);

            if (dr == true && File.Exists(this.SelectedFile)) { this.PicToAdd.Bitmap = File.ReadAllBytes(this.SelectedFile); }
        }

        #endregion Methods
    }
}