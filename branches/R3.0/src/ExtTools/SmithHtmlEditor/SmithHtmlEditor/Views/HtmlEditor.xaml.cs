namespace Smith.WPF.HtmlEditor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using System.Xml;
    using System.Xml.XPath;

    using log4net;

    using mshtml;

    public partial class HtmlEditor : UserControl
    {
        #region Fields

        public static readonly DependencyProperty BindingContentProperty = 
            DependencyProperty.Register("BindingContent", typeof(string), typeof(HtmlEditor),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingContentChanged)));
        public static readonly RoutedEvent DocumentReadyEvent = 
            EventManager.RegisterRoutedEvent("DocumentReady", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(HtmlEditor));
        public static readonly RoutedEvent DocumentStateChangedEvent = 
            EventManager.RegisterRoutedEvent("DocumentStateChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(HtmlEditor));
        public static readonly DependencyProperty EditModeProperty = 
            DependencyProperty.Register("EditMode", typeof(EditMode), typeof(HtmlEditor),
                new FrameworkPropertyMetadata(EditMode.Visual, new PropertyChangedCallback(OnEditModeChanged)));

        private static readonly string SourceFontFamilyPath = @"/smithhtmleditor/sourcemode/fontfamily/@value";
        private static readonly string SourceFontSizePath = @"/smithhtmleditor/sourcemode/fontsize/@value";
        private static readonly string VisualFontFamiliesPath = @"/smithhtmleditor/visualmode/fontfamilies/add/@value";

        RoutedEventArgs DocumentStateChangedEventArgs = new RoutedEventArgs(HtmlEditor.DocumentStateChangedEvent);
        private Window hostedWindow;
        private HtmlDocument htmldoc;
        private Dictionary<string, ImageObject> imageDic;
        bool isDocReady;
        private ILog logger = LogManager.GetLogger(typeof(HtmlEditor));
        private EditMode mode;
        private string myBindingContent = string.Empty;
        private string stylesheet;
        private DispatcherTimer styleTimer;

        #endregion Fields

        #region Constructors

        public HtmlEditor()
        {
            InitializeComponent();
            InitContainer();
            InitStyles();
            InitEvents();
            InitTimer();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Raise when the document is ready.
        /// </summary>
        public event RoutedEventHandler DocumentReady
        {
            add { base.AddHandler(DocumentReadyEvent, value); }
            remove { base.RemoveHandler(DocumentReadyEvent, value); }
        }

        /// <summary>
        /// Raise when the state of document is changed.
        /// </summary>
        public event RoutedEventHandler DocumentStateChanged
        {
            add { base.AddHandler(DocumentStateChangedEvent, value); }
            remove { base.RemoveHandler(DocumentStateChangedEvent, value); }
        }

        #endregion Events

        #region Properties

        public string BindingContent
        {
            get { return (string)GetValue(BindingContentProperty); }
            set { SetValue(BindingContentProperty, value); }
        }

        /// <summary>
        /// 获取一个值，指示复制命令是否可执行。
        /// Get a value that indicated if the copy command is enabled.
        /// </summary>
        public bool CanCopy
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Copy");
            }
        }

        /// <summary>
        /// 获取一个值，指示剪切命令是否可执行。
        /// Get a value that indicated if the cut command is enabled.
        /// </summary>
        public bool CanCut
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Cut");
            }
        }

        /// <summary>
        /// 获取一个值，指示删除命令是否可执行。
        /// Get a value that indicated if the delete command is enabled.
        /// </summary>
        public bool CanDelete
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Delete");
            }
        }

        /// <summary>
        /// 获取一个值，指示粘贴命令是否可执行。
        /// Get a value that indicated if the paste command is enabled.
        /// </summary>
        public bool CanPaste
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Paste");
            }
        }

        /// <summary>
        /// 获取一个值，指示重做命令是否可执行。
        /// Get a value that indicated if the redo command is enabled.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Redo");
            }
        }

        /// <summary>
        /// 获取一个值，撤销命令是否可执行。
        /// Get a value that indicated if the undo command is enabled.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                return
                    mode == EditMode.Visual &&
                    htmldoc != null &&
                    htmldoc.QueryCommandEnabled("Undo");
            }
        }

        /// <summary>
        /// 获取或设置编辑器中的HTML内容。
        /// Get or set the html content.
        /// </summary>
        public string ContentHtml
        {
            get
            {
                if (ToggleCodeMode.IsChecked == true)
                    VisualEditor.Document.Body.InnerHtml = CodeEditor.Text;
                return VisualEditor.Document.Body.InnerHtml;
            }
            set
            {
                value = (value != null ? value : string.Empty);
                BindingContent = value;
                if (VisualEditor.Document != null && VisualEditor.Document.Body != null)
                    VisualEditor.Document.Body.InnerHtml = value;

                if (ToggleCodeMode.IsChecked == true)
                    CodeEditor.Text = VisualEditor.Document.Body.InnerHtml;
            }
        }

        /// <summary>
        /// 获取编辑器中的文本内容。
        /// Get the text content.
        /// </summary>
        public string ContentText
        {
            get
            {
                if (ToggleCodeMode.IsChecked == true)
                    VisualEditor.Document.Body.InnerHtml = CodeEditor.Text;
                return VisualEditor.Document.Body.InnerText;
            }
        }

        /// <summary>
        /// 获取HTML文档对象。
        /// Get the html document of editor.
        /// </summary>
        public HtmlDocument Document
        {
            get { return htmldoc; }
        }

        /// <summary>
        /// Get or set the edit mode of editor.
        /// This is a dependency property.
        /// </summary>
        public EditMode EditMode
        {
            get { return (EditMode)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        /// <summary>
        /// 获取字数统计。
        /// Get word count.
        /// </summary>
        public int WordCount
        {
            get
            {
                // get word count in code editor when source mode is on
                if (ToggleCodeMode.IsChecked == true)
                {
                    WordCounter counter = WordCounter.Create();
                    return counter.Count(CodeEditor.Text);
                }
                // get word count in html editor when visual mode is on
                else if (htmldoc != null && htmldoc.Content != null)
                {
                    WordCounter counter = WordCounter.Create();
                    return counter.Count(htmldoc.Text);
                }
                return 0;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 执行复制命令。
        /// Execute the copy command.
        /// </summary>
        public void Copy()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Copy", false, null);
        }

        /// <summary>
        /// 执行剪切命令。
        /// Execute the cut command.
        /// </summary>
        public void Cut()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Cut", false, null);
        }

        /// <summary>
        /// 执行删除命令。
        /// Execute the delete command.
        /// </summary>
        public void Delete()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Delete", false, null);
        }

        /// <summary>
        /// 执行粘贴命令。
        /// Execute the paste command.
        /// </summary>
        public void Paste()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Paste", false, null);
        }

        /// <summary>
        /// 执行重做命令。
        /// Execute the redo command.
        /// </summary>
        public void Redo()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Redo", false, null);
        }

        /// <summary>
        /// 执行全选命令。
        /// Execute the select all command.
        /// </summary>
        public void SelectAll()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("SelectAll", false, null);
        }

        /// <summary>
        /// 执行撤销命令。
        /// Execute the undo command.
        /// </summary>
        public void Undo()
        {
            if (htmldoc != null)
                htmldoc.ExecuteCommand("Undo", false, null);
        }

        private static void OnBindingContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HtmlEditor editor = (HtmlEditor)sender;
            editor.myBindingContent = (string)e.NewValue;
            editor.ContentHtml = editor.myBindingContent;
        }

        private static void OnEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HtmlEditor editor = (HtmlEditor)sender;
            if ((EditMode)e.NewValue == EditMode.Visual) editor.SetVisualMode();
            else editor.SetSourceMode();
        }

        private void BoldExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Bold();
        }

        private void BubbledListExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.BulletsList();
        }

        private void ClearStyleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.ClearStyle();
        }

        private void CopyCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanCopy;
        }

        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Copy();
        }

        private void CutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanCut;
        }

        private void CutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Cut();
        }

        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanDelete;
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Delete();
        }

        private void EditingCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (htmldoc != null && mode == EditMode.Visual);
        }

        private ReadOnlyCollection<FontSize> GetDefaultFontSizes()
        {
            List<FontSize> ls = new List<FontSize>()
            {
                Smith.WPF.HtmlEditor.FontSize.XXSmall,
                Smith.WPF.HtmlEditor.FontSize.XSmall,
                Smith.WPF.HtmlEditor.FontSize.Small,
                Smith.WPF.HtmlEditor.FontSize.Middle,
                Smith.WPF.HtmlEditor.FontSize.Large,
                Smith.WPF.HtmlEditor.FontSize.XLarge,
                Smith.WPF.HtmlEditor.FontSize.XXLarge
            };
            return new ReadOnlyCollection<FontSize>(ls);
        }

        /// <summary>
        /// Get the content from editor.
        /// </summary>
        private string GetEditContent()
        {
            switch (mode)
            {
                case EditMode.Visual:
                    return VisualEditor.Document.Body.InnerHtml;
                default:
                    return CodeEditor.Text;
            }
        }

        private void IndentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Indent();
        }

        private void InitContainer()
        {
            LoadStylesheet();
            VisualEditor.Navigated += this.OnVisualEditorDocumentNavigated;
            VisualEditor.StatusTextChanged += this.OnVisualEditorStatusTextChanged;
            VisualEditor.DocumentText = String.Empty;
        }

        private void InitEvents()
        {
            this.Loaded += new RoutedEventHandler(OnHtmlEditorLoaded);
            this.Unloaded += new RoutedEventHandler(OnHtmlEditorUnloaded);
            ToggleFontColor.Click += new RoutedEventHandler(OnFontColorClick);
            ToggleLineColor.Click += new RoutedEventHandler(OnLineColorClick);
            ToggleCodeMode.Checked += new RoutedEventHandler(OnCodeModeChecked);
            ToggleCodeMode.Unchecked += new RoutedEventHandler(OnCodeModeUnchecked);
            FontColorContextMenu.Opened += new RoutedEventHandler(OnFontColorContextMenuOpened);
            FontColorContextMenu.Closed += new RoutedEventHandler(OnFontColorContextMenuClosed);
            LineColorContextMenu.Opened += new RoutedEventHandler(OnLineColorContextMenuOpened);
            LineColorContextMenu.Closed += new RoutedEventHandler(OnLineColorContextMenuClosed);
            FontColorPicker.SelectedColorChanged += new EventHandler<PropertyChangedEventArgs<Color>>(OnFontColorPickerSelectedColorChanged);
            LineColorPicker.SelectedColorChanged += new EventHandler<PropertyChangedEventArgs<Color>>(OnLineColorPickerSelectedColorChanged);
        }

        /// <summary>
        /// Initalize font families and font sizes of visual editor.
        /// Initalize font family and font size of code editor.
        /// </summary>
        private void InitStyles()
        {
            //ConfigProvider.Load();
            List<FontFamily> families = new List<FontFamily>();
            //List<FontSize> sizes = new List<FontSize>();
            FontFamily srcfamily = new FontFamily("Times New Roman");
            int srcsize = 10;

            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Smith.WPF.HtmlEditor.smithhtmleditor.config.xml");
                if (stream == null) throw new NullReferenceException("The embedded resource 'smithhtmleditor.config.xml' can't be loaded.");

                using (var reader = XmlReader.Create(stream))
                {
                    XPathDocument xmlDoc = new XPathDocument(reader);
                    XPathNavigator navDoc = xmlDoc.CreateNavigator();
                    XPathNodeIterator it;
                    // read visualmode/fontfamilies section
                    it = navDoc.Select(VisualFontFamiliesPath);
                    while (it.MoveNext())
                    {
                        FontFamily ff = new FontFamily(it.Current.Value);
                        families.Add(ff);
                    }
                    // read sourcemode/fontfamily section
                    it = navDoc.Select(SourceFontFamilyPath);
                    while (it.MoveNext())
                    {
                        srcfamily = new FontFamily(it.Current.Value);
                        break;
                    }
                    // read sourcemode/fontsize section
                    it = navDoc.Select(SourceFontSizePath);
                    while (it.MoveNext())
                    {
                        srcsize = it.Current.ValueAsInt;
                        break;
                    }
                }
                // set font families
                FontFamilyList.ItemsSource = new ReadOnlyCollection<FontFamily>(families);
                FontFamilyList.SelectionChanged += new SelectionChangedEventHandler(OnFontFamilyChanged);
            }
            catch (Exception ex)
            {
                this.logger.Error("An error occured while initialising styles", ex);
            }

            // set font sizes
            FontSizeList.ItemsSource = GetDefaultFontSizes();
            FontSizeList.SelectionChanged += new SelectionChangedEventHandler(OnFontSizeChanged);
            // set style of code editor
            CodeEditor.FontFamily = srcfamily;
            CodeEditor.FontSize = srcsize;
        }

        private void InitTimer()
        {
            styleTimer = new DispatcherTimer();
            styleTimer.Interval = TimeSpan.FromMilliseconds(200);
            styleTimer.Tick += new EventHandler(OnTimerTick);
        }

        private void InsertCodeBlockExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 插入超链接事件
        /// </summary>
        private void InsertHyperlinkExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null)
            {
                HyperlinkDialog d = new HyperlinkDialog();
                d.Owner = this.hostedWindow;
                d.Model = new HyperlinkObject { URL = "http://", Text = htmldoc.Selection.Text };
                if (d.ShowDialog() == true)
                {
                    htmldoc.InsertHyperlick(d.Model);
                }
            }
        }

        /// <summary>
        /// 插入图像事件
        /// </summary>
        private void InsertImageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null)
            {
                ImageDialog d = new ImageDialog();
                d.Owner = this.hostedWindow;
                if (d.ShowDialog() == true)
                {
                    htmldoc.InsertImage(d.Model);
                    imageDic[d.Model.ImageUrl] = d.Model;
                }
            }
        }

        /// <summary>
        /// 插入表格事件
        /// </summary>
        private void InsertTableExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null)
            {
                TableDialog d = new TableDialog();
                d.Owner = this.hostedWindow;
                if (d.ShowDialog() == true)
                {
                    htmldoc.InsertTable(d.Model);
                }
            }
        }

        private void ItalicExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Italic();
        }

        private void JustifyCenterExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.JustifyCenter();
        }

        private void JustifyFullExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.JustifyFull();
        }

        private void JustifyLeftExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.JustifyLeft();
        }

        private void JustifyRightExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.JustifyRight();
        }

        private void LoadStylesheet()
        {
            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Smith.WPF.HtmlEditor.smithhtmleditor.stylesheet.css");
                if (stream == null) throw new NullReferenceException("The embedded resource 'smithhtmleditor.stylesheet.css' can't be loaded.");

                using (var reader = new StreamReader(stream))
                {
                    stylesheet = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
            }
        }

        private void NotifyBindingContentChanged()
        {
            if (myBindingContent != this.ContentHtml)
            {
                BindingContent = this.ContentHtml;
            }
        }

        private void NumericListExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.NumberedList();
        }

        private void OnCodeModeChecked(object sender, RoutedEventArgs e)
        {
            EditMode = EditMode.Source;
        }

        private void OnCodeModeUnchecked(object sender, RoutedEventArgs e)
        {
            EditMode = EditMode.Visual;
        }

        private void OnDocumentContextMenuShowing(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            EditingContextMenu.IsOpen = true;
            e.ReturnValue = false;
        }

        private void OnFontColorClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fxElement = sender as FrameworkElement;
            if (fxElement != null && FontColorContextMenu != null)
            {
                FontColorContextMenu.PlacementTarget = fxElement;
                FontColorContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                FontColorContextMenu.IsOpen = true;
            }
        }

        private void OnFontColorContextMenuClosed(object sender, RoutedEventArgs e)
        {
            ToggleFontColor.IsChecked = false;
        }

        private void OnFontColorContextMenuOpened(object sender, RoutedEventArgs e)
        {
            FontColorPicker.Reset();
            ToggleFontColor.IsChecked = true;
        }

        private void OnFontColorPickerSelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e)
        {
            htmldoc.SetFontColor(e.NewValue);
        }

        /// <summary>
        /// Invoke when selected font family changed
        /// </summary>
        private void OnFontFamilyChanged(object sender, SelectionChangedEventArgs e)
        {
            if (htmldoc != null)
            {
                FontFamily selectionFontFamily = htmldoc.GetFontFamily();
                FontFamily selectedFontFamily = (FontFamily)FontFamilyList.SelectedValue;
                if (selectedFontFamily != selectionFontFamily) htmldoc.SetFontFamily(selectedFontFamily);
            }
        }

        /// <summary>
        /// Invoke when selected font size changed
        /// </summary>
        private void OnFontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (htmldoc != null)
            {
                FontSize selectionFontSize = htmldoc.GetFontSize();
                FontSize selectedFontSize = (FontSize)FontSizeList.SelectedValue;
                if (selectedFontSize != selectionFontSize) htmldoc.SetFontSize(selectedFontSize);
            }
        }

        private void OnHtmlEditorLoaded(object sender, RoutedEventArgs e)
        {
            imageDic = new Dictionary<string, ImageObject>();
            this.hostedWindow = this.GetParentWindow();
            styleTimer.Start();
        }

        private void OnHtmlEditorUnloaded(object sender, RoutedEventArgs e)
        {
            styleTimer.Stop();
        }

        private void OnLineColorClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement fxElement = sender as FrameworkElement;
            if (fxElement != null && LineColorContextMenu != null)
            {
                LineColorContextMenu.PlacementTarget = fxElement;
                LineColorContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                LineColorContextMenu.IsOpen = true;
            }
        }

        private void OnLineColorContextMenuClosed(object sender, RoutedEventArgs e)
        {
            ToggleLineColor.IsChecked = false;
        }

        private void OnLineColorContextMenuOpened(object sender, RoutedEventArgs e)
        {
            LineColorPicker.Reset();
            ToggleLineColor.IsChecked = true;
        }

        private void OnLineColorPickerSelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e)
        {
            htmldoc.SetLineColor(e.NewValue);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (htmldoc.State != HtmlDocumentState.Complete) return;

            ToggleBold.IsChecked = htmldoc.IsBold();
            ToggleItalic.IsChecked = htmldoc.IsItalic();
            ToggleUnderline.IsChecked = htmldoc.IsUnderline();
            ToggleSubscript.IsChecked = htmldoc.IsSubscript();
            ToggleSuperscript.IsChecked = htmldoc.IsSuperscript();
            ToggleBulletedList.IsChecked = htmldoc.IsBulletsList();
            ToggleNumberedList.IsChecked = htmldoc.IsNumberedList();
            ToggleJustifyLeft.IsChecked = htmldoc.IsJustifyLeft();
            ToggleJustifyRight.IsChecked = htmldoc.IsJustifyRight();
            ToggleJustifyCenter.IsChecked = htmldoc.IsJustifyCenter();
            ToggleJustifyFull.IsChecked = htmldoc.IsJustifyFull();

            FontFamilyList.SelectedItem = htmldoc.GetFontFamily();
            FontSizeList.SelectedItem = htmldoc.GetFontSize();
        }

        private void OnVisualEditorDocumentNavigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            VisualEditor.Document.ContextMenuShowing += this.OnDocumentContextMenuShowing;
            htmldoc = new HtmlDocument(VisualEditor.Document);
            //((IHTMLDocument2)VisualEditor.Document.DomDocument).designMode = "ON";
            SetStylesheet();
            SetInitialContent();
            VisualEditor.Document.Body.SetAttribute("contenteditable", "true");
            VisualEditor.Document.Focus();
        }

        private void OnVisualEditorStatusTextChanged(object sender, EventArgs e)
        {
            if (Document == null) return;

            RaiseEvent(DocumentStateChangedEventArgs);
            if (Document.State == HtmlDocumentState.Complete)
            {
                if (isDocReady)
                {
                    Dispatcher.BeginInvoke(new Action(this.NotifyBindingContentChanged));
                }
                else
                {
                    isDocReady = true;
                    RaiseEvent(new RoutedEventArgs(HtmlEditor.DocumentReadyEvent));
                }
            }
        }

        private void OutdentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Outdent();
        }

        private void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanPaste;
        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Paste();
        }

        private void RedoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanRedo;
        }

        private void RedoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Redo();
        }

        private void SelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectAll();
        }

        /// <summary>
        /// Set the inital content of editor
        /// </summary>
        private void SetInitialContent()
        {
            if (myBindingContent != null)
                VisualEditor.Document.Body.InnerHtml = myBindingContent;
        }

        /// <summary>
        /// Set the editor to source mode.
        /// </summary>
        private void SetSourceMode()
        {
            if (mode != EditMode.Source)
            {
                BrowserHost.Visibility = Visibility.Hidden;
                CodeEditor.Visibility = Visibility.Visible;

                FontFamilyList.IsEnabled = false;
                FontSizeList.IsEnabled = false;
                ToggleFontColor.IsEnabled = false;
                ToggleLineColor.IsEnabled = false;

                CodeEditor.Text = GetEditContent();
                mode = EditMode.Source;
            }
        }

        /// <summary>
        /// Set style for visual editor.
        /// </summary>
        private void SetStylesheet()
        {
            if (stylesheet != null && VisualEditor.Document != null)
            {
                HTMLDocument hdoc = (HTMLDocument)VisualEditor.Document.DomDocument;
                IHTMLStyleSheet hstyle = hdoc.createStyleSheet("", 0);
                hstyle.cssText = stylesheet;
            }
        }

        /// <summary>
        /// Set the editor to visual mode.
        /// </summary>
        private void SetVisualMode()
        {
            if (mode != EditMode.Visual)
            {
                BrowserHost.Visibility = Visibility.Visible;
                CodeEditor.Visibility = Visibility.Hidden;

                FontFamilyList.IsEnabled = true;
                FontSizeList.IsEnabled = true;
                ToggleFontColor.IsEnabled = true;
                ToggleLineColor.IsEnabled = true;

                VisualEditor.Document.Body.InnerHtml = GetEditContent();
                mode = EditMode.Visual;
            }
        }

        private void SubscriptCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mode == EditMode.Visual && htmldoc != null && htmldoc.CanSubscript());
        }

        private void SubscriptExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Subscript();
        }

        private void SuperscriptCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mode == EditMode.Visual && htmldoc != null && htmldoc.CanSuperscript());
        }

        private void SuperscriptExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Superscript();
        }

        private void UnderlineExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (htmldoc != null) htmldoc.Underline();
        }

        private void UndoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanUndo;
        }

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Undo();
        }

        #endregion Methods
    }
}