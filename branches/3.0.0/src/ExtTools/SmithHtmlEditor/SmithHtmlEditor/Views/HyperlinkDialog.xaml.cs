namespace Smith.WPF.HtmlEditor
{
    using System.Windows;

    /// <summary>
    /// HyperlinkDialog.xaml 的交互逻辑
    /// </summary>
    public partial class HyperlinkDialog : Window
    {
        #region Fields

        HyperlinkObject bindingContext;

        #endregion Fields

        #region Constructors

        public HyperlinkDialog()
        {
            InitializeComponent();

            Model = new HyperlinkObject
            {
                URL = "http://"
            };
            OkayButton.Click += new RoutedEventHandler(OkayButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #endregion Constructors

        #region Properties

        public HyperlinkObject Model
        {
            get { return bindingContext; }
            set
            {
                bindingContext = value;
                this.DataContext = bindingContext;
            }
        }

        #endregion Properties

        #region Methods

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Close();
        }

        void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal) this.DialogResult = true;
            this.Close();
        }

        #endregion Methods
    }
}