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
namespace Probel.NDoctor.View.Toolbox.Navigation
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;

    using Xceed.Wpf.Toolkit;

    public static class InnerWindow
    {
        #region Fields

        static string caption = "N.A.";
        private static System.Windows.Visibility closeButtonVisibility = System.Windows.Visibility.Visible;
        private static System.Windows.Controls.UserControl content;
        private static bool isModal = false;
        private static WindowStartupLocation windowStartupLocation;
        private static WindowState windowState = WindowState.Closed;

        #endregion Fields

        #region Constructors

        static InnerWindow()
        {
            CancelCommand = new RelayCommand(() => Close());
        }

        #endregion Constructors

        #region Events

        public static event EventHandler CaptionChanged;

        public static event EventHandler CloseButtonVisibilityChanged;

        public static event EventHandler Closed;

        public static event EventHandler ContentChanged;

        public static event EventHandler IsModalChanged;

        public static event EventHandler Loaded;

        public static event EventHandler WindowStartupLocationChanged;

        public static event EventHandler WindowStateChanged;

        #endregion Events

        #region Properties

        public static ICommand CancelCommand
        {
            get;
            private set;
        }

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

                if (value == WindowState.Open) OnLoaded();
                else if (value == WindowState.Closed) OnClosed();
            }
        }

        #endregion Properties

        #region Methods

        public static void Close()
        {
            WindowState = WindowState.Closed;
        }

        /// <summary>
        /// Shows the child window as a non modal form.
        /// </summary>
        /// <param name="caption">The caption. That's the title of the window</param>
        /// <param name="content">The content. That's the graphic interface contained in the child window</param>
        public static void Show(string caption, UserControl content)
        {
            Show(caption, content, false);
        }

        /// <summary>
        /// Shows the child window as a modal form.
        /// </summary>
        /// <param name="caption">The caption. That's the title of the window</param>
        /// <param name="content">The content. That's the graphic interface contained in the child window</param>
        public static void ShowDialog(string caption, UserControl content)
        {
            Show(caption, content, true);
        }

        private static void OnCaptionChanged()
        {
            if (CaptionChanged != null)
                CaptionChanged(new Object(), EventArgs.Empty);
        }

        private static void OnCloseButtonVisibilityChanged()
        {
            if (CloseButtonVisibilityChanged != null)
                CloseButtonVisibilityChanged(new Object(), EventArgs.Empty);
        }

        private static void OnClosed()
        {
            if (Closed != null)
                Closed(Content, EventArgs.Empty);
        }

        private static void OnContentChanged()
        {
            if (ContentChanged != null)
                ContentChanged(Content, EventArgs.Empty);
        }

        private static void OnIsModalChanged()
        {
            if (IsModalChanged != null)
                IsModalChanged(new Object(), EventArgs.Empty);
        }

        private static void OnLoaded()
        {
            if (Loaded != null)
                Loaded(Content, EventArgs.Empty);
        }

        private static void OnWindowStartupLocationChanged()
        {
            if (WindowStartupLocationChanged != null)
                WindowStartupLocationChanged(new Object(), EventArgs.Empty);
        }

        private static void OnWindowStateChanged()
        {
            if (WindowStateChanged != null)
                WindowStateChanged(new Object(), EventArgs.Empty);
        }

        private static void Show(string caption, UserControl content, bool isModal)
        {
            InnerWindow.Content = content;
            InnerWindow.WindowState = WindowState.Open;
            InnerWindow.IsModal = isModal;
            InnerWindow.Caption = caption;
        }

        #endregion Methods
    }
}