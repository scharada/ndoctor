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
namespace Probel.NDoctor.Plugins.PictureManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Conversions;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.MemoryComponents;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PictureManager.Helpers;
    using Probel.NDoctor.Plugins.PictureManager.Properties;
    using Probel.NDoctor.Plugins.PictureManager.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private readonly TagDto ALL_PICTURE_TAG = new TagDto(TagCategory.Picture) { Name = Messages.Msg_AllTags };
        private readonly ICommand selectPictureCommand;

        private IPictureComponent component;
        private bool creatingNewPicture = false;
        private TagDto filterTag;
        private bool isBusy;
        private bool isFilterDisabled = false;
        private bool isInformationExpanded;
        private bool isRefreshMuted = false;
        private LightPictureMemoryComponent memoryComponent = LightPictureMemoryComponent.Empty;
        private PictureDto selectedPicture;
        private TagDto selectedTag;
        private LightPictureDto selectedThumbnail;

        #endregion Fields

        #region Constructors

        public WorkbenchViewModel()
            : base()
        {
            this.IsBusy = true;
            this.Pictures = new ObservableCollection<LightPictureDto>();
            this.Tags = new ObservableCollection<TagDto>();
            this.FilterTags = new ObservableCollection<TagDto>();

            this.IsInformationExpanded = false;
            this.SelectedPicture = new PictureDto();
            this.component = PluginContext.ComponentFactory.GetInstance<IPictureComponent>();

            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPictureComponent>();
            PluginContext.Host.NewPatientConnected += (sender, e) => this.isRefreshMuted = false;

            Notifyer.ItemChanged += (sender, e) => this.Refresh();

            this.AddPictureCommand = new RelayCommand(() => AddPicture(), () => this.CanAddSomething());
            this.AddTypeCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPicType, new AddTagView()), () => this.CanAddSomething());
            this.SaveCommand = new RelayCommand(() => Save(), () => CanSave());
            this.FilterPictureCommand = new RelayCommand(() => this.Filter());
            this.selectPictureCommand = new RelayCommand(() => this.SelectPicture(), () => this.CanSelectPicture());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddPictureCommand
        {
            get;
            private set;
        }

        public ICommand AddTypeCommand
        {
            get;
            private set;
        }

        public string BtnSearch
        {
            get { return Messages.Title_BtnSearch; }
        }

        public ICommand FilterPictureCommand
        {
            get;
            private set;
        }

        public TagDto FilterTag
        {
            get { return this.filterTag; }
            set
            {
                this.filterTag = value;
                this.OnPropertyChanged(() => FilterTag);
            }
        }

        public ObservableCollection<TagDto> FilterTags
        {
            get;
            private set;
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

        public bool IsInformationExpanded
        {
            get { return this.isInformationExpanded; }
            set
            {
                this.isInformationExpanded = value;
                this.OnPropertyChanged(() => IsInformationExpanded);
            }
        }

        public ObservableCollection<LightPictureDto> Pictures
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public PictureDto SelectedPicture
        {
            get { return this.selectedPicture; }
            set
            {
                this.selectedPicture = value;

                // Select manually the tag of the selected picture
                SelectTagOfPicture();
                this.creatingNewPicture = false;

                this.OnPropertyChanged(() => SelectedPicture);
                this.OnPropertyChanged(() => TitleCreationDate);
                this.OnPropertyChanged(() => TitleLastUpdate);

                this.SetStatusToReady();
            }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        public LightPictureDto SelectedThumbnail
        {
            get { return this.selectedThumbnail; }
            set
            {
                this.selectedThumbnail = value;
                this.OnPropertyChanged(() => SelectedThumbnail);
            }
        }

        public ICommand SelectPictureCommand
        {
            get { return this.selectPictureCommand; }
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        public string TitleCreationDate
        {
            get
            {
                if (selectedPicture != null)
                {
                    return Messages.Title_CreationDate.FormatWith(this.SelectedPicture.Creation.Date.ToShortDateString());
                }
                else return string.Empty;
            }
        }

        public string TitleLastUpdate
        {
            get
            {
                if (selectedPicture != null)
                {
                    return Messages.Title_LastUpdate.FormatWith(this.SelectedPicture.LastUpdate);
                }
                else return string.Empty;
            }
        }

        public string TitleRest
        {
            get { return Messages.Title_Search; }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            //If there's no change since last time, don't reload the data.
            if (this.isRefreshMuted) { return; }
            try
            {
                this.SelectedPicture = new PictureDto();
                var tags = this.component.FindTags(TagCategory.Picture);

                this.isFilterDisabled = true;

                this.Tags.Refill(tags);
                this.InsertJokerTag(tags);
                this.FilterTags.Refill(tags);
                this.creatingNewPicture = false;
                this.SelectFirstTag();

                this.isFilterDisabled = false;

                this.Filter();
                this.isRefreshMuted = true; //We've just refresh, don't need to refresh next time.
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_ErrorFailToRefreshPicture);
            }
        }

        private void AddPicture()
        {
            var dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF",
            };
            var clickedOK = dialog.ShowDialog();
            if (clickedOK.HasValue && clickedOK.Value)
            {
                this.IsInformationExpanded = true;
                var bytes = Converter.FileToByteArray(dialog.FileName);
                this.SelectedPicture = new PictureDto();
                this.SelectedPicture.Bitmap = bytes;
                this.creatingNewPicture = true;
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PictureAdded);
                this.creatingNewPicture = true;
            }
            else return;
        }

        private bool CanAddSomething()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSave()
        {
            return (this.SelectedPicture != null
                  && this.selectedPicture.Bitmap != null
                  && this.SelectedPicture.Bitmap.Length > 0
                  && this.SelectedTag != null)
                  && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSelectPicture()
        {
            return true;
        }

        private void Filter()
        {
            if (this.isFilterDisabled)
            {
                this.Logger.Debug("Picture filter canceled...");
                return;
            }

            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var input = new TaskArgs()
            {
                SelectedPatient = PluginContext.Host.SelectedPatient,
                MemoryComponent = this.memoryComponent,
            };

            this.IsBusy = true;
            if (!this.isRefreshMuted)
            {
                Task.Factory
                    .StartNew<TaskArgs>(e => this.FilterAsync(e as TaskArgs), input as TaskArgs)
                    .ContinueWith(e => this.FilterCallback(e.Result), context);
            }
            else //If nothing has changed, optimise filter doing this in memory
            {
                Task.Factory
                    .StartNew<Tuple<LightPictureDto[], PictureDto>>(() => this.FilterInMemoryAsync())
                    .ContinueWith(e => this.FilterInMemoryCallback(e), context);
            }
        }

        private TaskArgs FilterAsync(TaskArgs input)
        {
            Thread.CurrentThread.CurrentUICulture = input.CurrentUICulture;

            if (this.FilterTag != null && this.FilterTag.Name == Messages.Msg_AllTags)
            {
                input.Pictures = this.component.FindLightPictures(input.SelectedPatient);
                input.MemoryComponent = new LightPictureMemoryComponent(input.Pictures);
            }
            else
            {
                input.Pictures = this.component.FindLightPictures(input.SelectedPatient, input.FilterTag);
            }

            if (input.Pictures.Count > 0)
            {
                input.SelectedPicture = component.FindPicture(input.Pictures[0]);
            }
            else { input.SelectedPicture = null; }

            this.Logger.Debug("\tThread finished: Filtered pictures");
            return input;
        }

        private void FilterCallback(TaskArgs args)
        {
            this.memoryComponent = args.MemoryComponent ?? this.memoryComponent;
            this.Pictures.Refill(args.Pictures);
            this.SelectedPicture = args.SelectedPicture;
            this.Logger.Debug("\tRefreshed GUI after pictures filtering");
            this.IsBusy = false;
        }

        private Tuple<LightPictureDto[], PictureDto> FilterInMemoryAsync()
        {
            var foundItems = new LightPictureDto[] { };
            var currentPic = new PictureDto();

            if (this.FilterTag != null && this.FilterTag.Name != Messages.Msg_AllTags)
            {
                foundItems = memoryComponent.Find(this.FilterTag);
            }
            else { foundItems = memoryComponent.FindAll(); }

            if (foundItems.Length > 0)
            {
                currentPic = component.FindPicture(foundItems[0]);
            }
            else { currentPic = null; }

            return new Tuple<LightPictureDto[], PictureDto>(foundItems, currentPic);
        }

        private void FilterInMemoryCallback(Task<Tuple<LightPictureDto[], PictureDto>> e)
        {
            this.Pictures.Refill(e.Result.Item1);
            this.SelectedPicture = e.Result.Item2;
            this.IsBusy = false;
            this.Logger.Debug("Filtered pictures in memory");
        }

        private void InsertJokerTag(IList<TagDto> tags)
        {
            var count = (from tag in tags
                         where tag.Name == Messages.Msg_AllTags
                         select tag).Count();

            if (count == 0)
            {
                tags.Insert(0, ALL_PICTURE_TAG);
            }
        }

        private void Save()
        {
            try
            {
                this.IsBusy = true;
                this.SelectedPicture.LastUpdate = DateTime.Now;
                this.SelectedPicture.Tag = this.SelectedTag;

                Assert.IsNotNull(this.SelectedTag, "All pictures should have a tag");

                var context = TaskScheduler.FromCurrentSynchronizationContext();
                var args = new TaskArgs()
                {
                    SelectedPatient = PluginContext.Host.SelectedPatient,
                    SelectedPicture = this.SelectedPicture,
                    CreatingNewPicture = this.creatingNewPicture,
                };

                Task.Factory
                    .StartNew<TaskArgs>(e => SaveAsync(e as TaskArgs), args)
                    .ContinueWith(e => this.SaveCallback(e.Result), context);
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_ErOnSavePicture);
            }
        }

        private TaskArgs SaveAsync(TaskArgs context)
        {
            if (context.CreatingNewPicture)
            {
                this.component.Create(context.SelectedPicture, context.SelectedPatient);
                context.CreatingNewPicture = false;
            }
            else { this.component.Update(context.SelectedPicture); }
            return context;
        }

        private void SaveCallback(TaskArgs context)
        {
            this.isRefreshMuted = false;
            this.Refresh();

            this.creatingNewPicture = context.CreatingNewPicture;
            this.IsBusy = false;

            PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PictureUpdated);
            this.SelectedPicture = new PictureDto();
            this.IsInformationExpanded = false;
        }

        private void SelectFirstTag()
        {
            if (this.FilterTags.Count > 0)
            {
                this.FilterTag = this.FilterTags[0];
            }
        }

        private void SelectPicture()
        {
            if (this.selectedThumbnail != null)
            {
                this.SelectedPicture = this.component.FindPicture(this.SelectedThumbnail);
            }
        }

        private void SelectTagOfPicture()
        {
            if (this.SelectedPicture == null) return;
            if (this.SelectedPicture.Tag == null) return;

            this.SelectedTag = (from t in this.FilterTags
                                where t.Id == this.SelectedPicture.Tag.Id
                                select t).FirstOrDefault();
        }

        #endregion Methods

        #region Nested Types

        private class TaskArgs
        {
            #region Constructors

            public TaskArgs()
            {
                this.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            }

            #endregion Constructors

            #region Properties

            public bool CreatingNewPicture
            {
                get;
                set;
            }

            public CultureInfo CurrentUICulture
            {
                get;
                private set;
            }

            public TagDto FilterTag
            {
                get;
                set;
            }

            public LightPictureMemoryComponent MemoryComponent
            {
                get;
                set;
            }

            public IList<LightPictureDto> Pictures
            {
                get;
                set;
            }

            public LightPatientDto SelectedPatient
            {
                get;
                set;
            }

            public PictureDto SelectedPicture
            {
                get;
                set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}