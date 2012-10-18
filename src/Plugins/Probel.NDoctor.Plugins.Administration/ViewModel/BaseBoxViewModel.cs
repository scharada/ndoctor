namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Helpers;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.Services.Messaging;

    internal abstract class BaseBoxViewModel<T> : BaseViewModel
        where T : BaseDto
    {
        #region Fields

        private readonly IAdministrationComponent component;

        private T boxItem;

        #endregion Fields

        #region Constructors

        public BaseBoxViewModel()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
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
                Notifyer.OnRefreshing(this);
                InnerWindow.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_ItemAdded);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanAdd()
        {
            return this.BoxItem != null
                && this.BoxItem.IsValid()
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        #endregion Methods
    }
}