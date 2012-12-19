namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;

    internal abstract class BaseBoxViewModel<T> : BaseViewModel
        where T : BaseDto
    {
        #region Fields

        private readonly ICommand closeCommand;
        private readonly IAdministrationComponent component;

        private T boxItem;

        #endregion Fields

        #region Constructors

        public BaseBoxViewModel()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
            this.closeCommand = new RelayCommand(() => this.Close());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            internal set; //The setter is open to allow versatility
        }

        public T BoxItem
        {
            get { return this.boxItem; }
            set
            {
                this.boxItem = value;
                this.OnPropertyChanged(() => BoxItem);
            }
        }

        public ICommand CloseCommand
        {
            get { return this.closeCommand; }
        }

        public IAdministrationComponent Component
        {
            get { return this.component; }
        }

        #endregion Properties

        #region Methods

        protected abstract void AddItem();

        private void Add()
        {
            try
            {
                this.AddItem();
                this.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_ItemAdded);
            }
            catch (ExistingItemException ex) { this.Handle.Warning(ex, ex.TranslatedMessage); }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanAdd()
        {
            return this.BoxItem != null
                && this.BoxItem.IsValid()
                && PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.BoxItem.IsValid();
        }

        #endregion Methods
    }
}