namespace Smith.WPF.HtmlEditor
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Documents;

    /// <summary>
    /// TableDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TableDialog : Window
    {
        #region Fields

        ReadOnlyCollection<TableAlignment> alignmentOptions;
        TableObject bindingContext;
        ReadOnlyCollection<TableHeaderOption> headerOptions;
        ReadOnlyCollection<Unit> unitOptions;

        #endregion Fields

        #region Constructors

        public TableDialog()
        {
            InitializeComponent();
            InitUnitOptions();
            InitHeaderOptions();
            InitAlignmentOptions();
            InitEvents();
            InitBindingContext();
        }

        #endregion Constructors

        #region Properties

        public TableObject Model
        {
            get { return bindingContext; }
            private set
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

        void InitAlignmentOptions()
        {
            List<TableAlignment> ls = new List<TableAlignment>()
            {
                TableAlignment.Default,
                TableAlignment.Left,
                TableAlignment.Right,
                TableAlignment.Center
            };
            alignmentOptions = new ReadOnlyCollection<TableAlignment>(ls);
            AlignmentSelection.ItemsSource = alignmentOptions;
        }

        void InitBindingContext()
        {
            Model = new TableObject
            {
                Columns = 5,
                Rows = 3,
                Border = 1,
                Width = 100,
                Height = 100,
                WidthUnit = Unit.Percentage,
                HeightUnit = Unit.Pixel,
                SpacingUnit = Unit.Pixel,
                PaddingUnit = Unit.Pixel,
                HeaderOption = TableHeaderOption.Default,
                Alignment = TableAlignment.Default
            };
        }

        void InitEvents()
        {
            OkayButton.Click += new RoutedEventHandler(OkayButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void InitHeaderOptions()
        {
            List<TableHeaderOption> ls = new List<TableHeaderOption>
            {
                TableHeaderOption.Default,
                TableHeaderOption.FirstRow,
                TableHeaderOption.FirstColumn,
                TableHeaderOption.FirstRowAndColumn
            };
            headerOptions = new ReadOnlyCollection<TableHeaderOption>(ls);
            HeaderSelection.ItemsSource = headerOptions;
        }

        void InitUnitOptions()
        {
            List<Unit> ls = new List<Unit> { Unit.Pixel, Unit.Percentage };
            unitOptions = new ReadOnlyCollection<Unit>(ls);

            WidthUnitSelection.ItemsSource = ls;
            HeightUnitSelection.ItemsSource = ls;
            SpaceUnitSelection.ItemsSource = ls;
            PaddingUnitSelection.ItemsSource = ls;
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