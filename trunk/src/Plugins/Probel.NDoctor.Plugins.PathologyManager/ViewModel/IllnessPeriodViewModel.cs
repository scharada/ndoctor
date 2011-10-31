namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class IllnessPeriodViewModel : IllnessPeriodDto
    {
        #region Fields

        private IPathologyComponent component = ComponentFactory.PathologyComponent;
        private bool isSelected;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IllnessPeriodViewModel"/> class.
        /// </summary>
        public IllnessPeriodViewModel()
        {
            this.RemoveCommand = new RelayCommand(() =>
            {
                var dr = MessageBox.Show(Messages.Msg_DeleteIllnessPeriod, Messages.Title_Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes)
                {
                    using (this.component.UnitOfWork)
                    {
                        this.component.Remove(this, PluginContext.Host.SelectedPatient);
                    }
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_IllnessRemoved);
                    this.OnRefreshed(State.Removed);
                }
            });
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when this item needs to be refreshed.
        /// </summary>
        public event EventHandler<EventArgs<State>> Refreshed;

        #endregion Events

        #region Properties

        public string DurationAsText
        {
            get
            {
                var span = this.End - this.Start;
                return string.Format(Messages.Title_IllnessDuration, span.TotalDays);
            }
        }

        public string EndAsText
        {
            get { return this.End.ToShortDateString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected in the ListView.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public string StartAsText
        {
            get { return this.Start.ToShortDateString(); }
        }

        #endregion Properties

        #region Methods

        private void OnRefreshed(State state)
        {
            if (this.Refreshed != null)
                this.Refreshed(this, new EventArgs<State>(state));
        }

        #endregion Methods
    }
}