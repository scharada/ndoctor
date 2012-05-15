namespace Smith.WPF.HtmlEditor
{
    public class ImageAlignment : OptionObject
    {
        #region Fields

        public static readonly ImageAlignment Bottom = 
            new ImageAlignment { Text = Resources.UiText.Align_Bottom, Value = "bottom" };
        public static readonly ImageAlignment Center = 
            new ImageAlignment { Text = Resources.UiText.Align_Center, Value = "center" };
        public static readonly ImageAlignment Default = 
            new ImageAlignment { Text = Resources.UiText.Align_Default, Value = "" };
        public static readonly ImageAlignment Left = 
            new ImageAlignment { Text = Resources.UiText.Align_Left, Value = "left" };
        public static readonly ImageAlignment Right = 
            new ImageAlignment { Text = Resources.UiText.Align_Right, Value = "right" };
        public static readonly ImageAlignment Top = 
            new ImageAlignment { Text = Resources.UiText.Align_Top, Value = "top" };

        #endregion Fields

        #region Constructors

        protected ImageAlignment()
        {
        }

        #endregion Constructors
    }

    public class OptionObject
    {
        #region Properties

        public string Text
        {
            get; protected set;
        }

        public string Value
        {
            get; protected set;
        }

        #endregion Properties
    }

    public class TableAlignment : OptionObject
    {
        #region Fields

        public static readonly TableAlignment Center = 
            new TableAlignment { Text = Resources.UiText.Align_Center, Value = "center" };
        public static readonly TableAlignment Default = 
            new TableAlignment { Text = Resources.UiText.Align_Default, Value = "" };
        public static readonly TableAlignment Left = 
            new TableAlignment { Text = Resources.UiText.Align_Left, Value = "left" };
        public static readonly TableAlignment Right = 
            new TableAlignment { Text = Resources.UiText.Align_Right, Value = "right" };

        #endregion Fields

        #region Constructors

        protected TableAlignment()
        {
        }

        #endregion Constructors
    }

    public class TableHeaderOption : OptionObject
    {
        #region Fields

        public static readonly TableHeaderOption Default = 
            new TableHeaderOption { Text = Resources.UiText.Header_Default, Value = "Default" };
        public static readonly TableHeaderOption FirstColumn = 
            new TableHeaderOption { Text = Resources.UiText.Header_FirstColumn, Value = "FirstColumn" };
        public static readonly TableHeaderOption FirstRow = 
            new TableHeaderOption { Text = Resources.UiText.Header_FirstRow, Value = "FirstRow" };
        public static readonly TableHeaderOption FirstRowAndColumn = 
            new TableHeaderOption { Text = Resources.UiText.Header_FirstRowAndColumn, Value = "FirstRowAndColumn" };

        #endregion Fields

        #region Constructors

        protected TableHeaderOption()
        {
        }

        #endregion Constructors
    }

    public class Unit : OptionObject
    {
        #region Fields

        public static readonly Unit Percentage = new Unit { Text = "%", Value = "%" };
        public static readonly Unit Pixel = new Unit { Text = "px", Value = "px" };

        #endregion Fields

        #region Constructors

        protected Unit()
        {
        }

        #endregion Constructors
    }
}