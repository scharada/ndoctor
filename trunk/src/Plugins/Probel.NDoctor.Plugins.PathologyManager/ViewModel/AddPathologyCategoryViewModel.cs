namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Helpers;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddPathologyCategoryViewModel : BaseViewModel
    {
        #region Fields

        private static readonly IPathologyComponent Component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();

        private string categoryName;

        #endregion Fields

        #region Constructors

        public AddPathologyCategoryViewModel()
        {
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string CategoryName
        {
            get { return this.categoryName; }
            set
            {
                this.categoryName = value;
                this.OnPropertyChanged(() => CategoryName);
            }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                var tag = new TagDto(TagCategory.Appointment) { Name = this.CategoryName };
                using (Component.UnitOfWork)
                {
                    Component.Create(tag);
                }
                InnerWindow.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_CategoryAdded);
                Notifyer.OnItemChanged(this);
                InnerWindow.Close();
            }
            catch (ExistingItemException ex) { this.HandleWarning(ex, ex.Message); }
            catch (Exception ex)
            {
                this.HandleError(ex);
                InnerWindow.Close();
            }
        }

        private bool CanAdd()
        {
            return this.CategoryName != null;
        }

        #endregion Methods
    }
}