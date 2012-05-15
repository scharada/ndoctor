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
namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.Events;

    /// <summary>
    /// Class used for binding of a RibbonControl
    /// </summary>
    public class RibbonControlData : RibbonBase
    {
        #region Fields

        private ICommand command;
        private string keyTip;
        private string label;
        private Uri largeImage;
        private Uri smallImage;
        private string toolTipDescription;
        private string toolTipFooterDescription;
        private Uri toolTipFooterImage;
        private string toolTipFooterTitle;
        private Uri toolTipImage;
        private string toolTipTitle;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonControlData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        public RibbonControlData(string label, string uriImage)
            : this(label, uriImage, null)
        {
        }

        public RibbonControlData(string label, ICommand command)
            : this(label, null, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonControlData"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="uriImage">The URI image.</param>
        /// <param name="command">The command.</param>
        public RibbonControlData(string label, string uriImage, ICommand command)
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

        protected RibbonControlData()
        {
        }

        #endregion Constructors

        #region Properties

        public ICommand Command
        {
            get { return this.command; }
            set
            {
                if (this.command != value)
                {
                    this.command = value;
                    this.OnPropertyChanged("Command");
                }
            }
        }

        public string KeyTip
        {
            get { return this.keyTip; }
            set
            {
                if (this.keyTip != value)
                {
                    this.keyTip = value;
                    this.OnPropertyChanged("KeyTip");
                }
            }
        }

        public string Label
        {
            get { return this.label; }
            set
            {
                if (this.label != value)
                {
                    this.label = value;
                    this.OnPropertyChanged("Label");
                }
            }
        }

        public Uri LargeImage
        {
            get { return this.largeImage; }
            set
            {
                if (this.largeImage != value)
                {
                    this.largeImage = value;
                    this.OnPropertyChanged("LargeImage");
                }
            }
        }

        public Uri SmallImage
        {
            get { return this.smallImage; }
            set
            {
                if (this.smallImage != value)
                {
                    this.smallImage = value;
                    this.OnPropertyChanged("SmallImage");
                }
            }
        }

        public string ToolTipDescription
        {
            get { return this.toolTipDescription; }
            set
            {
                if (this.toolTipDescription != value)
                {
                    this.toolTipDescription = value;
                    this.OnPropertyChanged("ToolTipDescription");
                }
            }
        }

        public string ToolTipFooterDescription
        {
            get { return this.toolTipFooterDescription; }
            set
            {
                if (this.toolTipFooterDescription != value)
                {
                    this.toolTipFooterDescription = value;
                    this.OnPropertyChanged("ToolTipFooterDescription");
                }
            }
        }

        public Uri ToolTipFooterImage
        {
            get { return this.toolTipFooterImage; }
            set
            {
                if (this.toolTipFooterImage != value)
                {
                    this.toolTipFooterImage = value;
                    this.OnPropertyChanged("ToolTipFooterImage");
                }
            }
        }

        public string ToolTipFooterTitle
        {
            get { return this.toolTipFooterTitle; }
            set
            {
                if (this.toolTipFooterTitle != value)
                {
                    this.toolTipFooterTitle = value;
                    this.OnPropertyChanged("ToolTipFooterTitle");
                }
            }
        }

        public Uri ToolTipImage
        {
            get { return this.toolTipImage; }
            set
            {
                if (this.toolTipImage != value)
                {
                    this.toolTipImage = value;
                    this.OnPropertyChanged("ToolTipImage");
                }
            }
        }

        public string ToolTipTitle
        {
            get { return this.toolTipTitle; }
            set
            {
                if (this.toolTipTitle != value)
                {
                    this.toolTipTitle = value;
                    this.OnPropertyChanged("ToolTipTitle");
                }
            }
        }

        #endregion Properties
    }
}