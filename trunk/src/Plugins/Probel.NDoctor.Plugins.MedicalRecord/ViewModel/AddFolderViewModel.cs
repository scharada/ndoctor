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
namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddFolderViewModel : BaseViewModel
    {
        #region Fields

        private IMedicalRecordComponent component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private TagDto tagToAdd;

        #endregion Fields

        #region Constructors

        public AddFolderViewModel()
            : base()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();

            this.tagToAdd = new TagDto(TagCategory.MedicalRecord);
            this.AddFolderCommand = new RelayCommand(() =>
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.tagToAdd);
                }
                InnerWindow.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_TagAdded.FormatWith(this.tagToAdd.Name));

                Notifyer.OnRefreshed();

            }, () => !string.IsNullOrWhiteSpace(this.tagToAdd.Name));
        }

        #endregion Constructors

        #region Properties

        public ICommand AddFolderCommand
        {
            get;
            private set;
        }

        public TagDto TagToAdd
        {
            get { return this.tagToAdd; }
            set
            {
                this.tagToAdd = value;
                this.OnPropertyChanged(() => TagToAdd);
            }
        }

        #endregion Properties
    }
}