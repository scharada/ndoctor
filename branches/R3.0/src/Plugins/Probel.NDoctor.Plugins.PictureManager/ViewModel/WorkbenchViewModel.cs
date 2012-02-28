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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Conversions;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PictureManager.Helpers;
    using Probel.NDoctor.Plugins.PictureManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IPictureComponent component;
        private bool creatingNewPicture = false;
        private TagDto filterTag;
        private bool isInformationExpanded;
        private ObservableCollection<PictureDto> pictures = new ObservableCollection<PictureDto>();
        private PictureDto selectedPicture;
        private TagDto selectedTag;
        private ObservableCollection<TagDto> tags = new ObservableCollection<TagDto>();

        #endregion Fields

        #region Constructors

        public WorkbenchViewModel()
            : base()
        {
            this.IsInformationExpanded = false;
            this.SelectedPicture = new PictureDto();
            this.component = ObjectFactory.GetInstance<IPictureComponent>();

            this.AddPictureCommand = new RelayCommand(() => AddPicture(), () => PluginContext.Host.SelectedPatient != null);
            this.SaveCommand = new RelayCommand(() => Save(), () => CheckSave());
            this.FilterPictureCommand = new RelayCommand(() => this.Refresh());

            Notifyer.ItemChanged += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ICommand AddPictureCommand
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
                this.OnPropertyChanged("FilterTag");

                this.Logger.DebugFormat("The filter tag changed. Filter is null: {0}", (value == null));
            }
        }

        public bool IsInformationExpanded
        {
            get { return this.isInformationExpanded; }
            set
            {
                this.isInformationExpanded = value;
                this.OnPropertyChanged("IsInformationExpanded");
            }
        }

        public ObservableCollection<PictureDto> Pictures
        {
            get { return this.pictures; }
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

                this.OnPropertyChanged("SelectedPicture", "TitleCreationDate", "TitleLastUpdate");
                this.SetStatusToReady();
            }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged("SelectedTag");
            }
        }

        public ICommand SelectPictureCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TagDto> Tags
        {
            get { return this.tags; }
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
            try
            {
                this.SelectedPicture = new PictureDto();

                IList<TagDto> tags;
                IList<PictureDto> pictures;

                using (this.component.UnitOfWork)
                {
                    tags = this.component.FindTags(TagCategory.Picture);
                    pictures = this.component.FindPictures(PluginContext.Host.SelectedPatient, this.FilterTag);
                }
                this.Tags.Refill(tags);
                this.Pictures.Refill(pictures);
                this.creatingNewPicture = false;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorFailToRefreshPicture);
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

        private bool CheckSave()
        {
            return (this.SelectedPicture != null
                  && this.selectedPicture.Bitmap != null
                  && this.SelectedPicture.Bitmap.Length > 0
                  && this.SelectedTag != null);
        }

        private void Save()
        {
            try
            {
                this.SelectedPicture.LastUpdate = DateTime.Now;
                this.SelectedPicture.Tag = this.SelectedTag;

                Assert.IsNotNull(this.SelectedTag, "All pictures should have a tag");
                using (this.component.UnitOfWork)
                {
                    if (this.creatingNewPicture)
                    {
                        this.component.Create(this.SelectedPicture, PluginContext.Host.SelectedPatient);
                        this.creatingNewPicture = false;
                    }
                    else this.component.Update(this.SelectedPicture);
                }

                this.Logger.Debug("Updating picture");

                this.Refresh();

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PictureUpdated);
                this.SelectedPicture = new PictureDto();
                this.IsInformationExpanded = false;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErOnSavePicture);
            }
        }

        private void SelectTagOfPicture()
        {
            if (this.SelectedPicture == null) return;
            if (this.SelectedPicture.Tag == null) return;

            this.SelectedTag = (from t in this.Tags
                                where t.Id == this.SelectedPicture.Tag.Id
                                select t).FirstOrDefault();
        }

        #endregion Methods
    }
}