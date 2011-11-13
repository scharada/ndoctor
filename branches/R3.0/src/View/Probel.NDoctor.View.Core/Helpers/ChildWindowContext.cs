namespace Probel.NDoctor.View.Core.Helpers
{
    using System;

    using Microsoft.Windows.Controls;

    public static class ChildWindowContext
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

        public static event EventHandler WindowStartupLocationChanged;

        public static event EventHandler WindowStateChanged;

        #endregion Events

        #region Properties

        public static string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnCaptionChanged();
            }
        }

        public static System.Windows.Visibility CloseButtonVisibility
        {
            get { return closeButtonVisibility; }
            set
            {
                closeButtonVisibility = value;
                OnCloseButtonVisibilityChanged();
            }
        }

        public static System.Windows.Controls.UserControl Content
        {
            get { return content; }
            set
            {
                content = value;
                OnContentChanged();
            }
        }

        public static bool IsModal
        {
            get { return isModal; }
            set
            {
                isModal = value;
                OnIsModalChanged();
            }
        }

        public static WindowStartupLocation WindowStartupLocation
        {
            get { return windowStartupLocation; }
            set
            {
                windowStartupLocation = value;
                OnWindowStartupLocationChanged();
            }
        }

        public static WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                OnWindowStateChanged();
            }
        }

        #endregion Properties

        #region Methods

        public static void OnCaptionChanged()
        {
            if (CaptionChanged != null)
                CaptionChanged(null, EventArgs.Empty);
        }

        public static void OnCloseButtonVisibilityChanged()
        {
            if (CloseButtonVisibilityChanged != null)
                CloseButtonVisibilityChanged(null, EventArgs.Empty);
        }

        public static void OnIsModalChanged()
        {
            if (IsModalChanged != null)
                IsModalChanged(null, EventArgs.Empty);
        }

        public static void OnWindowStartupLocationChanged()
        {
            if (WindowStartupLocationChanged != null)
                WindowStartupLocationChanged(null, EventArgs.Empty);
        }

        public static void OnWindowStateChanged()
        {
            if (WindowStateChanged != null)
                WindowStateChanged(null, EventArgs.Empty);
        }

        private static void OnContentChanged()
        {
            if (ContentChanged != null)
                ContentChanged(null, EventArgs.Empty);
        }

        #endregion Methods
    }
}