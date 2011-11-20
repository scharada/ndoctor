namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    public class RibbonComboBoxData : RibbonMenuButtonData
    {
        #region Fields

        private object dataContext;
        private double selectionBoxWidth;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonComboBoxData"/> class.
        /// </summary>
        public RibbonComboBoxData()
        {
            this.SelectionBoxWidth = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonComboBoxData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonComboBoxData(string label, string uriImage, ICommand command)
            : base(label, uriImage, command)
        {
            this.SelectionBoxWidth = -1;
        }

        #endregion Constructors

        #region Properties

        public object DataContext
        {
            get { return this.dataContext; }
            set
            {
                this.dataContext = value;
                this.OnPropertyChanged("DataContext");
            }
        }

        public double SelectionBoxWidth
        {
            get { return this.selectionBoxWidth; }
            set
            {
                this.selectionBoxWidth = value;
                this.OnPropertyChanged("SelectionBoxWidth");
            }
        }

        #endregion Properties
    }
}