namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System.Windows.Input;

    public class RibbonToggleButtonData : RibbonControlData
    {
        #region Fields

        private bool isChecked;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonToggleButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonToggleButtonData(string label, string uriImage, ICommand command)
            : base(label, uriImage, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonToggleButtonData"/> class.
        /// </summary>
        public RibbonToggleButtonData()
            : base()
        {
        }

        #endregion Constructors

        #region Properties

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }

        #endregion Properties
    }
}