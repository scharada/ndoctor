namespace Smith.WPF.HtmlEditor
{
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    public class HtmlObject : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }

    public class HyperlinkObject : HtmlObject
    {
        #region Fields

        string fdText;
        string fdURL;

        #endregion Fields

        #region Properties

        public string Text
        {
            get { return fdText; }
            set
            {
                fdText = value;
                RaisePropertyChanged("Text");
            }
        }

        public string URL
        {
            get { return fdURL; }
            set
            {
                fdURL = value;
                RaisePropertyChanged("URL");
            }
        }

        #endregion Properties
    }

    public class ImageObject : HtmlObject
    {
        #region Fields

        ImageAlignment fdAlignment;
        string fdAltText;
        int fdBorderSize;
        int fdHeight;
        int fdHorizontalSpace;
        BitmapImage fdImage;
        string fdImageUrl;
        string fdLinkUrl;
        int fdOriginalHeight;
        int fdOriginalWidth;
        string fdTitleText;
        int fdVerticalSpace;
        int fdWidth;

        #endregion Fields

        #region Properties

        public ImageAlignment Alignment
        {
            get { return fdAlignment; }
            set
            {
                fdAlignment = value;
                RaisePropertyChanged("Alignment");
            }
        }

        public string AltText
        {
            get { return fdAltText; }
            set
            {
                fdAltText = value;
                RaisePropertyChanged("AlternativeText");
            }
        }

        public int BorderSize
        {
            get { return fdBorderSize; }
            set
            {
                fdBorderSize = value;
                RaisePropertyChanged("BorderSize");
            }
        }

        public int Height
        {
            get { return fdHeight; }
            set
            {
                fdHeight = value;
                RaisePropertyChanged("Height");
            }
        }

        public int HorizontalSpace
        {
            get { return fdHorizontalSpace; }
            set
            {
                fdHorizontalSpace = value;
                RaisePropertyChanged("HorizontalSpace");
            }
        }

        public BitmapImage Image
        {
            get { return fdImage; }
            set
            {
                fdImage = value;
                RaisePropertyChanged("Image");
            }
        }

        public string ImageUrl
        {
            get { return fdImageUrl; }
            set
            {
                fdImageUrl = value;
                RaisePropertyChanged("ImageUrl");
            }
        }

        public string LinkUrl
        {
            get { return fdLinkUrl; }
            set
            {
                fdLinkUrl = value;
                RaisePropertyChanged("LinkUrl");
            }
        }

        public int OriginalHeight
        {
            get { return fdOriginalHeight; }
            set
            {
                fdOriginalHeight = value;
                RaisePropertyChanged("OriginalHeight");
            }
        }

        public int OriginalWidth
        {
            get { return fdOriginalWidth; }
            set
            {
                fdOriginalWidth = value;
                RaisePropertyChanged("OriginalWidth");
            }
        }

        public string TitleText
        {
            get { return fdTitleText; }
            set
            {
                fdTitleText = value;
                RaisePropertyChanged("Title");
            }
        }

        public int VerticalSpace
        {
            get { return fdVerticalSpace; }
            set
            {
                fdVerticalSpace = value;
                RaisePropertyChanged("VerticalSpace");
            }
        }

        public int Width
        {
            get { return fdWidth; }
            set
            {
                fdWidth = value;
                RaisePropertyChanged("Width");
            }
        }

        #endregion Properties
    }

    public class TableObject : HtmlObject
    {
        #region Fields

        TableAlignment fdAlignment;
        int fdBorder;
        int fdColumns;
        TableHeaderOption fdHeaderOption;
        int fdHeight;
        Unit fdHeightUnit;
        int fdPadding;
        Unit fdPaddingUnit;
        int fdRows;
        int fdSpacing;
        Unit fdSpacingUnit;
        string fdTitle;
        int fdWidth;
        Unit fdWidthUnit;

        #endregion Fields

        #region Properties

        public TableAlignment Alignment
        {
            get { return fdAlignment; }
            set
            {
                fdAlignment = value;
                RaisePropertyChanged("Alignment");
            }
        }

        public int Border
        {
            get { return fdBorder; }
            set
            {
                fdBorder = value;
                RaisePropertyChanged("Border");
            }
        }

        public int Columns
        {
            get { return fdColumns; }
            set
            {
                if (value <= 0) value = 1;
                fdColumns = value;
                RaisePropertyChanged("Columns");
            }
        }

        public TableHeaderOption HeaderOption
        {
            get { return fdHeaderOption; }
            set
            {
                fdHeaderOption = value;
                RaisePropertyChanged("HeaderOption");
            }
        }

        public int Height
        {
            get { return fdHeight; }
            set
            {
                fdHeight = value;
                RaisePropertyChanged("Height");
            }
        }

        public Unit HeightUnit
        {
            get { return fdHeightUnit; }
            set
            {
                fdHeightUnit = value;
                RaisePropertyChanged("HeightUnit");
            }
        }

        public int Padding
        {
            get { return fdPadding; }
            set
            {
                fdPadding = value;
                RaisePropertyChanged("Padding");
            }
        }

        public Unit PaddingUnit
        {
            get { return fdPaddingUnit; }
            set
            {
                fdPaddingUnit = value;
                RaisePropertyChanged("PaddingUnit");
            }
        }

        public int Rows
        {
            get { return fdRows; }
            set
            {
                if (value <= 0) value = 1;
                fdRows = value;
                RaisePropertyChanged("Rows");
            }
        }

        public int Spacing
        {
            get { return fdSpacing; }
            set
            {
                fdSpacing = value;
                RaisePropertyChanged("Spacing");
            }
        }

        public Unit SpacingUnit
        {
            get { return fdSpacingUnit; }
            set
            {
                fdSpacingUnit = value;
                RaisePropertyChanged("SpacingUnit");
            }
        }

        public string Title
        {
            get { return fdTitle; }
            set
            {
                fdTitle = value;
                RaisePropertyChanged("Title");
            }
        }

        public int Width
        {
            get { return fdWidth; }
            set
            {
                fdWidth = value;
                RaisePropertyChanged("Width");
            }
        }

        public Unit WidthUnit
        {
            get { return fdWidthUnit; }
            set
            {
                fdWidthUnit = value;
                RaisePropertyChanged("WidthUnit");
            }
        }

        #endregion Properties
    }
}