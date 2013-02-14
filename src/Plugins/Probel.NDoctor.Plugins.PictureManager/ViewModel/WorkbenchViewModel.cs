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

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PictureManager.Properties;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private readonly TagDto ALL_PICTURE_TAG = new TagDto(TagCategory.Picture) { Name = Messages.Msg_AllTags };
        private readonly ICommand editCommand;
        private readonly ICommand selectPictureCommand;
        private readonly ICommand updateCommand;

        private IPictureComponent component;
        private TagDto filterTag;
        private bool isBusy;
        private bool isEditing;
        private bool isInformationExpanded;
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

            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPictureComponent>();

            this.refreshCommand = new RelayCommand(() => this.Refresh());
            this.AddPictureCommand = new RelayCommand(() => AddPicture(), () => this.CanAddSomething());
            this.AddTypeCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddTagViewModel>(), () => this.CanAddSomething());
            this.FilterPictureCommand = new RelayCommand(() => this.Filter());
            this.selectPictureCommand = new RelayCommand(() => this.SelectPicture(), () => this.CanSelectPicture());
            this.editCommand = new RelayCommand(() => this.Edit(), () => this.CanEdit());
            this.updateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
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

        public ICommand EditCommand
        {
            get { return this.editCommand; }
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
                if (value != null)
                {
                    this.filterTag = value;
                    this.OnPropertyChanged(() => FilterTag);
                }
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

        public bool IsEditing
        {
            get { return this.isEditing; }
            set
            {
                this.isEditing = value;
                this.OnPropertyChanged(() => IsEditing);
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

        public PictureDto SelectedPicture
        {
            get { return this.selectedPicture; }
            set
            {
                this.selectedPicture = value;

                // Select manually the tag of the selected picture
                SelectTagOfPicture();

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
                    return Messages.Title_LastUpdate.FormatWith(this.SelectedPicture.LastUpdate.ToShortDateString());
                }
                else return string.Empty;
            }
        }

        public string TitleRest
        {
            get { return Messages.Title_Search; }
        }

        public ICommand UpdateCommand
        {
            get { return this.updateCommand; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes that data, but it doesn't trigger a filter
        /// </summary>
        public void RefreshForNavigation()
        {
            this.Refresh(false);
        }

        private void AddPicture()
        {
            ViewService.Manager.ShowDialog<AddPictureViewModel>();
        }

        private bool CanAddSomething()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanEdit()
        {
            return !this.IsEditing
                && this.SelectedPicture != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSelectPicture()
        {
            return true;
        }

        private bool CanUpdate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditing;
        }

        private void Edit()
        {
            this.IsEditing = true;
        }

        private void Filter()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;
            var input = new TaskArgs()
            {
                SelectedPatient = PluginContext.Host.SelectedPatient,
                FilterTag = this.FilterTag,
            };
            this.IsBusy = true;
            var task = Task.Factory.StartNew<TaskArgs>(e => this.FilterAsync(e as TaskArgs), input);
            task.ContinueWith(e => this.FilterCallback(e), token, TaskContinuationOptions.OnlyOnRanToCompletion, context);
            task.ContinueWith(e => this.Handle.Error(e.Exception.InnerExceptions), token, TaskContinuationOptions.OnlyOnFaulted, context);
        }

        private TaskArgs FilterAsync(TaskArgs input)
        {
            Thread.CurrentThread.CurrentUICulture = input.CurrentUICulture;

            if (input.FilterTag != null)
            {

                this.Logger.DebugFormat("Filtering on '{0}'", input.FilterTag.Name);

                input.Pictures = (input.FilterTag.Name == Messages.Msg_AllTags)
                    ? this.component.GetLightPictures(input.SelectedPatient)
                    : this.component.GetLightPictures(input.SelectedPatient, input.FilterTag);

                if (input.Pictures.Count > 0)
                {
                    input.SelectedPicture = component.GetPicture(input.Pictures[0]);
                }
                else { input.SelectedPicture = null; }

                this.Logger.Debug("\tThread finished: Filtered pictures");
                return input;
            }
            else
            {
                this.Logger.Debug("\tThread finished: Null filter");
                input.Pictures = new List<LightPictureDto>();
                return input;
            }
        }

        private void FilterCallback(Task<TaskArgs> e)
        {
            var args = e.Result;
            this.Pictures.Refill(args.Pictures);
            this.SelectedPicture = args.SelectedPicture;
            this.Logger.Debug("\tRefreshed GUI after pictures filtering");
            this.IsBusy = false;
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

        /// <summary>
        /// Refreshes the data and execute a filter based on the selected filter.
        /// </summary>
        /// <param name="doFilter">if set to <c>true</c> execute a  filter on the picture based on the selected tag.</param>
        private void Refresh(bool doFilter = true)
        {
            try
            {
                this.SelectedPicture = new PictureDto();
                var tags = this.component.GetTags(TagCategory.Picture);

                this.Tags.Refill(tags);
                this.InsertJokerTag(tags);
                this.FilterTags.Refill(tags);
                this.SelectFirstTag();

                if (doFilter) { this.Filter(); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }


        private readonly ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }


        private void SelectFirstTag()
        {
            if (this.FilterTags.Count > 0)
            {
                this.FilterTag = this.FilterTags[0];
            }
            else { this.Logger.Warn("There's no tags in the picture filter ComboBox"); }
        }

        private void SelectPicture()
        {
            if (this.selectedThumbnail != null)
            {
                this.SelectedPicture = this.component.GetPicture(this.SelectedThumbnail);
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

        private void Update()
        {
            this.IsEditing = false;

            this.IsBusy = true;
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            this.SelectedPicture.Tag = this.SelectedTag;
            var task = Task.Factory.StartNew(e => UpdateAsync(e), this.SelectedPicture);
            task.ContinueWith(e => this.Filter(), token, TaskContinuationOptions.OnlyOnRanToCompletion, context);
            task.ContinueWith(e => this.Handle.Error(e.Exception.InnerException ?? e.Exception), token, TaskContinuationOptions.OnlyOnFaulted, context);
        }

        private void UpdateAsync(object e)
        {
            if (e is PictureDto)
            {
                var picture = e as PictureDto;
                this.component.Update(picture);
            }
            else { this.Logger.WarnFormat("Update a picture is expecting a 'PictureDto' but a '{0}' was secified", e.GetType().Name); }
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