namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddPathologyCategoryViewModel : BaseViewModel
    {
        #region Fields

        private static readonly IPathologyComponent Component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();

        private TagDto tag = new TagDto(TagCategory.Pathology);

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

        public TagDto Tag
        {
            get { return this.tag; }
            set
            {
                this.tag = value;
                this.OnPropertyChanged(() => Tag);
            }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            try
            {
                using (Component.UnitOfWork)
                {
                    Component.Create(this.Tag);
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
            finally { InnerWindow.Close(); }
        }

        private bool CanAdd()
        {
            return this.Tag != null;
        }

        #endregion Methods
    }
}