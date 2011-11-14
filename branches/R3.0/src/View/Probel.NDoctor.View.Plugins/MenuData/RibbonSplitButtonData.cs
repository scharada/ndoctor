namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    public class RibbonSplitButtonData : RibbonMenuButtonData
    {
        #region Fields

        private RibbonButtonData dropDownButtonData;
        private bool isCheckable;
        private bool isChecked;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        public RibbonSplitButtonData()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="isApplicationMenu">if set to <c>true</c> [is application menu].</param>
        public RibbonSplitButtonData(bool isApplicationMenu)
            : base(isApplicationMenu)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        public RibbonSplitButtonData(string label, string uriImage)
            : this(label, uriImage, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="command">The command.</param>
        public RibbonSplitButtonData(string label, ICommand command)
            : this(label, null, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonSplitButtonData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonSplitButtonData(string label, string uriImage, ICommand command)
        {
            this.Label = label;
            this.Command = command;

            if (!string.IsNullOrEmpty(uriImage))
            {
                this.LargeImage
                    = this.SmallImage
                    = new Uri(uriImage, UriKind.RelativeOrAbsolute);
            }
        }

        #endregion Constructors

        #region Properties

        public RibbonButtonData DropDownButtonData
        {
            get
            {
                if (dropDownButtonData == null)
                {
                    dropDownButtonData = new RibbonButtonData();
                }

                return dropDownButtonData;
            }
        }

        public bool IsCheckable
        {
            get
            {
                return isCheckable;
            }

            set
            {
                if (isCheckable != value)
                {
                    isCheckable = value;
                    OnPropertyChanged("IsCheckable");
                }
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        #endregion Properties
    }
}