namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Windows.Controls;

    using Microsoft.Windows.Controls;

    public static class InnerWindow
    {
        #region Fields

        static string caption = "N.A.";
        private static System.Windows.Visibility closeButtonVisibility = System.Windows.Visibility.Visible;
        private static System.Windows.Controls.UserControl content;
        private static bool isModal = false;
        private static Microsoft.Windows.Controls.WindowStartupLocation windowStartupLocation;
        private static WindowState windowState = WindowState.Closed;

        #endregion Fields

        #region Events

        public static event EventHandler CaptionChanged;

        public static event EventHandler CloseButtonVisibilityChanged;

        public static event EventHandler ContentChanged;

        public static event EventHandler IsModalChanged;

        public static event EventHandler Loaded;

        public static event EventHandler WindowStartupLocationChanged;

        public static event EventHandler WindowStateChanged;

        #endregion Events

        #region Properties

        public static string Caption
        {
            get { return caption; }
            internal set
            {
                caption = value;
                OnCaptionChanged();
            }
        }

        public static System.Windows.Visibility CloseButtonVisibility
        {
            get { return closeButtonVisibility; }
            internal set
            {
                closeButtonVisibility = value;
                OnCloseButtonVisibilityChanged();
            }
        }

        public static System.Windows.Controls.UserControl Content
        {
            get { return content; }
            internal set
            {
                content = value;
                OnContentChanged();
            }
        }

        public static bool IsModal
        {
            get { return isModal; }
            internal set
            {
                isModal = value;
                OnIsModalChanged();
            }
        }

        public static WindowStartupLocation WindowStartupLocation
        {
            get { return windowStartupLocation; }
            internal set
            {
                windowStartupLocation = value;
                OnWindowStartupLocationChanged();
            }
        }

        public static WindowState WindowState
        {
            get { return windowState; }
            internal set
            {
                windowState = value;
                OnWindowStateChanged();

                if (value == WindowState.Open)
                    OnLoaded();
            }
        }

        #endregion Properties

        #region Methods

        public static void Close()
        {
            WindowState = Microsoft.Windows.Controls.WindowState.Closed;
        }

        public static void OnCaptionChanged()
        {
            if (CaptionChanged != null)
                CaptionChanged(new Object(), EventArgs.Empty);
        }

        public static void OnCloseButtonVisibilityChanged()
        {
            if (CloseButtonVisibilityChanged != null)
                CloseButtonVisibilityChanged(new Object(), EventArgs.Empty);
        }

        public static void OnIsModalChanged()
        {
            if (IsModalChanged != null)
                IsModalChanged(new Object(), EventArgs.Empty);
        }

        public static void OnWindowStartupLocationChanged()
        {
            if (WindowStartupLocationChanged != null)
                WindowStartupLocationChanged(new Object(), EventArgs.Empty);
        }

        public static void OnWindowStateChanged()
        {
            if (WindowStateChanged != null)
                WindowStateChanged(new Object(), EventArgs.Empty);
        }

        /// <summary>
        /// Shows the child window as a non modal form.
        /// </summary>
        /// <param name="caption">The caption. That's the title of the window</param>
        /// <param name="content">The content. That's the graphic interface contained in the child window</param>
        public static void Show(string caption, UserControl content)
        {
            InnerWindow.Content = content;
            InnerWindow.WindowState = WindowState.Open;
            InnerWindow.IsModal = false;
            InnerWindow.Caption = caption;
        }

        private static void OnContentChanged()
        {
            if (ContentChanged != null)
                ContentChanged(new Object(), EventArgs.Empty);
        }

        private static void OnLoaded()
        {
            if (Loaded != null)
                Loaded(new Object(), EventArgs.Empty);
        }

        #endregion Methods
    }
}